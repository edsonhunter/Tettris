using System;
using System.Collections;
using System.Collections.Generic;
using Tettris.Controller.Shape;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scenes.Interface;
using Tettris.Services.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayScene : BaseScene<GameplayScene.GamePlayData>
{
    [Header("")]
    [SerializeField]
    private List<TetrominoController> TetrominoPrefabs = null;

    [SerializeField]
    private TetrominoController _currentTetromino = null;

    [SerializeField]
    private GameObject _startPosition = null;

    [SerializeField]
    private GameObject _background = null;

    [SerializeField]
    private Camera _camera = null;

    private IGameService GameService { get; set; }
    private IList<TetrominoController> TetrominosInPlay { get; set; }

    protected override void Loading(Action<bool> loaded)
    {
        GameService = GetService<IGameService>();
        GameService.CreateNewBoard(SceneData.Altura, SceneData.Largura);
        loaded(true);
    }
    
    protected override void Loaded()
    {
        StartNewTetromino();
        StartCoroutine(Turno());
        TetrominosInPlay = new List<TetrominoController>();
    }

    private void StartNewTetromino()
    {
        _currentTetromino = null;
        var tetromino = GameService.StartNewTetromino();
        _currentTetromino = Instantiate(TetrominoPrefabs[Random.Range(0, TetrominoPrefabs.Count)], _startPosition.transform.position, Quaternion.identity);
        _currentTetromino.Init(tetromino);
    }

    protected override void Loop()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameService.Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameService.Move(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameService.Move(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameService.Rotate(Quaternion.Euler(0, 0, 90f));
        }
    }

    private IEnumerator Turno()
    {
        while (GameService.Running)
        {
            if (!GameService.NextTurno())
            {
                if (CompletedLine())
                {
                    yield return new WaitForSeconds(0.5f);
                }

                if (GameService.GameOver())
                {
                    break;
                }

                StartNewTetromino();
            }

            yield return new WaitForSeconds(1f);

            break;
        }

        GetManager<ISceneManager>().LoadSceneOverlay(new EndGameData(200));
    }

    private bool CompletedLine()
    {
        _currentTetromino.End();
        TetrominosInPlay.Add(_currentTetromino);
        var completedLines = GameService.CompleteLine();
        if (completedLines.Count <= 0)
        {
            return false;
        }

        var tetrominosDestruidos = new List<TetrominoController>();
        for (int lineIdx = 0; lineIdx < completedLines.Count; lineIdx++)
        {
            foreach (TetrominoController controller in TetrominosInPlay)
            {
                controller.ClearLine(completedLines[lineIdx]);
                controller.RowDown(completedLines[lineIdx]);

                if (controller.Cubes.Count <= 0)
                {
                    tetrominosDestruidos.Add(controller);
                }
            }
        }

        foreach (var destruido in tetrominosDestruidos)
        {
            TetrominosInPlay.Remove(destruido);
        }

        return true;
    }

    public class GamePlayData : ISceneData
    {
        public int Altura { get; private set; }
        public int Largura { get; private set; }

        public GamePlayData(int altura, int largura)
        {
            Altura = altura;
            Largura = largura;
        }
    }
}
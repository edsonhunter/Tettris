using System;
using System.Collections;
using System.Collections.Generic;
using Tettris.Controller.Shape;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scenes.Interface;
using Tettris.Services.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField]
    private TextMeshProUGUI _score = null;
    private TextMeshProUGUI Score => _score;

    [SerializeField]
    private Button _pauseButton = null;
    private Button PauseButton => _pauseButton;

    private int CurrentScore { get; set; }
    
    protected override void Loading(Action<bool> loaded)
    {
        GameService = GetService<IGameService>();
        GameService.CreateNewBoard(SceneData.Altura, SceneData.Largura);
        PauseButton.onClick.AddListener(OnPauseClick);
        
        loaded(true);
    }
    
    protected override void Loaded()
    {
        StartCoroutine(GetManager<IAudioManager>().FadeIn(2f));
        NextRound();
        StartCoroutine(Turno());
        TetrominosInPlay = new List<TetrominoController>();
        CurrentScore = 0;
        _score.text = $"SCORE: {CurrentScore.ToString()}";
    }

    private void NextRound()
    {
        _currentTetromino = null;
        var tetromino = GameService.NextRound();
        _currentTetromino = Instantiate(TetrominoPrefabs[Random.Range(0, TetrominoPrefabs.Count)], _startPosition.transform.position, Quaternion.identity);
        _currentTetromino.Init(tetromino);
    }

    protected override void Loop()
    {
        if (!GameService.Running)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameService.Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameService.Move(Vector3.right);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
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
        while (GameService.GameOver())
        {
            if (!GameService.NextTurno())
            {
                if (CompletedLine())
                {
                    _score.text = $"SCORE: {CurrentScore.ToString()}";
                    yield return new WaitForSeconds(0.5f);
                }

                NextRound();
            }

            yield return new WaitForSeconds(GameService.Speed());

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
        foreach (var lines in completedLines)
        {
            foreach (var controller in TetrominosInPlay)
            {
                if (controller.ClearLine(lines))
                {
                    CurrentScore += 100;
                }
                controller.RowDown(lines);

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

    private void OnPauseClick()
    {
        GetManager<ISceneManager>().LoadSceneOverlay(new InGameMenuData());
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
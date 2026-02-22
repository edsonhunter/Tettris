using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tettris.Controller.Shape;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scenes.Interface;
using Tettris.ScriptableObject;
using Tettris.Services.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScene : BaseScene<GameplayScene.GamePlayData>
{
    [Header("")]
    [SerializeField]
    private CubeData _cubeData = null;
    
    private Queue<Cube> _cubePool = new Queue<Cube>();
    private CancellationTokenSource _dropCts;

    [SerializeField]
    private GameObject _startPosition = null;

    [SerializeField]
    private GameObject _background = null;

    [SerializeField]
    private Camera _camera = null;

    private IGameService GameService { get; set; }

    [SerializeField]
    private TextMeshProUGUI _score = null;
    private TextMeshProUGUI Score => _score;
    
    [SerializeField]
    private TextMeshProUGUI _level = null;
    private TextMeshProUGUI Level => _level;

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
        
        _dropCts = new CancellationTokenSource();
        _ = TurnoAsync();
        CurrentScore = 0;
        _level.text = $"LEVEL: {GameService.CurrentLevel}";
        _score.text = $"SCORE: {CurrentScore.ToString()}";
    }

    private void NextRound()
    {
        var tetromino = GameService.NextRound();
        Material material = _cubeData.Materials[(int)tetromino.TetrominoType];
        
        foreach (var baseTetromino in tetromino.BaseTetrominos)
        {
            Cube cube = GetCube();
            cube.transform.position = _startPosition.transform.position;
            cube.SetMaterial(material);
            cube.Init(baseTetromino, ReturnCube);
        }
    }

    private Cube GetCube()
    {
        if (_cubePool.Count > 0)
        {
            var cube = _cubePool.Dequeue();
            cube.gameObject.SetActive(true);
            return cube;
        }
        return Instantiate(_cubeData.CubePrefab, _startPosition.transform.position, Quaternion.identity).GetComponent<Cube>();
    }
    
    private void ReturnCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        _cubePool.Enqueue(cube);
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
            _dropCts?.Cancel();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameService.Rotate(Quaternion.Euler(0, 0, 90f));
        }
    }

    private async Task TurnoAsync()
    {
        while (!GameService.GameOver())
        {
            if (!GameService.NextTurno())
            {
                if (CompletedLine())
                {
                    await Task.Delay(500);
                    _score.text = $"SCORE: {CurrentScore.ToString()}";
                }

                NextRound();
                _level.text = $"LEVEL: {GameService.CurrentLevel}";
            }

            try
            {
                await Task.Delay((int)(GameService.Speed() * 1000), _dropCts.Token);
            }
            catch (TaskCanceledException)
            {
                _dropCts = new CancellationTokenSource();
            }
        }

        GetManager<ISceneManager>().LoadSceneOverlay(new EndGameData(200));
    }

    private bool CompletedLine()
    {
        var completedLines = GameService.CompleteLine();
        if (completedLines.Count <= 0)
        {
            return false;
        }

        CurrentScore += completedLines.Count * 100;

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
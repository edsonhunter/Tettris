using System;
using System.Threading;
using System.Threading.Tasks;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scenes.Interface;
using Tettris.Services.Interface;
using UnityEngine;
using Tettris.Scenes.Gameplay;

public class GameplayScene : BaseScene<GameplayScene.GamePlayData>
{
    [Header("Handlers")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private GameplayUIHandler _uiHandler;
    [SerializeField] private TetrisInputHandler _inputHandler;
    [SerializeField] private BoardRenderer _boardRenderer;
    [SerializeField] private CameraController _cameraController;

    private CancellationTokenSource _dropCts;

    private IGameService GameService { get; set; }
    private int CurrentScore { get; set; }
    
    protected override void Loading(Action<bool> loaded)
    {
        GameService = GetService<IGameService>();
        GameService.CreateNewBoard(SceneData.Altura, SceneData.Largura);
        
        _boardRenderer.Initialize(SceneData.Largura, SceneData.Altura);
        _cameraController.Initialize(SceneData.Largura, SceneData.Altura, _inputHandler);
        _uiHandler.Initialize(OnPauseClick);
        SubscribeToInput();

        loaded(true);
    }
    
    protected override void Loaded()
    {
        StartCoroutine(GetManager<IAudioManager>().FadeIn(2f));
        
        var tetromino = GameService.NextRound();
        _cubeSpawner.SpawnTetromino(tetromino);
        
        _dropCts = new CancellationTokenSource();
        _ = TurnoAsync();
        
        CurrentScore = 0;
        _uiHandler.UpdateLevel(GameService.CurrentLevel);
        _uiHandler.UpdateScore(CurrentScore);
    }

    private void SubscribeToInput()
    {
        _inputHandler.OnMoveLeft += InputHandler_OnMoveLeft;
        _inputHandler.OnMoveRight += InputHandler_OnMoveRight;
        _inputHandler.OnRotate += InputHandler_OnRotate;
        _inputHandler.OnFastDropStart += InputHandler_OnFastDropStart;
        _inputHandler.OnFastDropEnd += InputHandler_OnFastDropEnd;
    }

    private void InputHandler_OnMoveLeft() => GameService.Move(Vector3.left);
    private void InputHandler_OnMoveRight() => GameService.Move(Vector3.right);
    private void InputHandler_OnRotate() => GameService.Rotate(Quaternion.Euler(0, 0, -90f));
    
    private void InputHandler_OnFastDropStart()
    {
        GameService.IsFastDropping = true;
        _dropCts?.Cancel();
    }

    private void InputHandler_OnFastDropEnd()
    {
        GameService.IsFastDropping = false;
        _dropCts?.Cancel();
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveLeft -= InputHandler_OnMoveLeft;
            _inputHandler.OnMoveRight -= InputHandler_OnMoveRight;
            _inputHandler.OnRotate -= InputHandler_OnRotate;
            _inputHandler.OnFastDropStart -= InputHandler_OnFastDropStart;
            _inputHandler.OnFastDropEnd -= InputHandler_OnFastDropEnd;
        }
    }

    protected override void Loop()
    {
        _inputHandler.Loop();
        _cameraController.Loop();
    }

    private async Task TurnoAsync()
    {
        while (!GameService.GameOver())
        {
            if (!GameService.NextTurno())
            {
                _cameraController.DropShake();

                if (CompletedLine())
                {
                    await Task.Delay(500, _dropCts.Token);
                    _uiHandler.UpdateScore(CurrentScore);
                }

                var nextTetromino = GameService.NextRound();
                _cubeSpawner.SpawnTetromino(nextTetromino);
                _uiHandler.UpdateLevel(GameService.CurrentLevel);
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
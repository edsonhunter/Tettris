using System;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scenes.Interface;
using Tettris.Services.Interface;
using UnityEngine;
using Tettris.Scenes.Gameplay;
using Tettris.Domain.Interface.Tetronimo;

public class GameplayScene : BaseScene<GameplayScene.GamePlayData>
{
    [Header("Handlers")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private GameplayUIHandler _uiHandler;
    [SerializeField] private TetrisInputHandler _inputHandler;
    [SerializeField] private BoardRenderer _boardRenderer;
    [SerializeField] private CameraController _cameraController;

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
        
        CurrentScore = 0;
        _uiHandler.UpdateLevel(GameService.CurrentLevel);
        _uiHandler.UpdateScore(CurrentScore);

        SubscribeToGameService();
        _ = GameService.StartGameAsync(destroyCancellationToken);
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
    }

    private void InputHandler_OnFastDropEnd()
    {
        GameService.IsFastDropping = false;
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

        UnsubscribeFromGameService();
    }

    protected override void Loop()
    {
        _inputHandler.Loop();
        _cameraController.Loop();
    }

    private void SubscribeToGameService()
    {
        if (GameService == null) return;
        GameService.OnTetrominoSpawned += GameService_OnTetrominoSpawned;
        GameService.OnPieceLanded += GameService_OnPieceLanded;
        GameService.OnLinesCleared += GameService_OnLinesCleared;
        GameService.OnLevelChanged += GameService_OnLevelChanged;
        GameService.OnGameOver += GameService_OnGameOver;
    }

    private void UnsubscribeFromGameService()
    {
        if (GameService == null) return;
        GameService.OnTetrominoSpawned -= GameService_OnTetrominoSpawned;
        GameService.OnPieceLanded -= GameService_OnPieceLanded;
        GameService.OnLinesCleared -= GameService_OnLinesCleared;
        GameService.OnLevelChanged -= GameService_OnLevelChanged;
        GameService.OnGameOver -= GameService_OnGameOver;
    }

    private void GameService_OnTetrominoSpawned(ITetromino tetromino)
    {
        _cubeSpawner.SpawnTetromino(tetromino);
    }

    private void GameService_OnPieceLanded()
    {
        _cameraController.DropShake();
    }

    private void GameService_OnLinesCleared(int score)
    {
        CurrentScore += score;
        _uiHandler.UpdateScore(CurrentScore);
    }

    private void GameService_OnLevelChanged(float level)
    {
        _uiHandler.UpdateLevel(level);
    }

    private void GameService_OnGameOver()
    {
        GetManager<ISceneManager>().LoadSceneOverlay(new EndGameData(CurrentScore)); 
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
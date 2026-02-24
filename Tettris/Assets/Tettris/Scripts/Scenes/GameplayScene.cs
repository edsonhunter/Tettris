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
    [SerializeField] private ShaderVariantCollection _shaderVariants;

    private IGameService GameService { get; set; }
    private int CurrentScore { get; set; }
    private ITetromino _activeTetromino;
    
    protected override void Loading(Action<bool> loaded)
    {
        if (_shaderVariants != null)
        {
            if (!_shaderVariants.isWarmedUp)
            {
                _shaderVariants.WarmUp();
            }
        }

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

    private void InputHandler_OnMoveLeft() => GameService.Move(new System.Numerics.Vector2(-1, 0));
    private void InputHandler_OnMoveRight() => GameService.Move(new System.Numerics.Vector2(1, 0));
    private void InputHandler_OnRotate() => GameService.Rotate(-90f);
    
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

        UnsubscribeToActiveTetromino();
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

    private void SubscribeToActiveTetromino()
    {
        _activeTetromino.OnMove += ActiveTetromino_OnMove;
        _activeTetromino.OnRotate += ActiveTetromino_OnRotate;
    }

    private void UnsubscribeToActiveTetromino()
    {
        if (_activeTetromino != null)
        {
            _activeTetromino.OnMove -= ActiveTetromino_OnMove;
            _activeTetromino.OnRotate -= ActiveTetromino_OnRotate;
            _activeTetromino = null;
        }
    }

    private void GameService_OnTetrominoSpawned(ITetromino tetromino)
    {
        _cubeSpawner.SpawnTetromino(tetromino);

        UnsubscribeToActiveTetromino();

        _activeTetromino = tetromino;
        SubscribeToActiveTetromino();
        UpdateBoardHighlights();
    }

    private void GameService_OnPieceLanded()
    {
        _cameraController.DropShake();
        UnsubscribeToActiveTetromino();
        _boardRenderer.HighlightColumns(null);
    }

    private void ActiveTetromino_OnMove(object sender, System.Numerics.Vector2 e)
    {
        UpdateBoardHighlights();
    }

    private void ActiveTetromino_OnRotate(object sender, System.Collections.Generic.IList<System.Numerics.Vector2> e)
    {
        UpdateBoardHighlights();
    }

    private void UpdateBoardHighlights()
    {
        if (_activeTetromino == null || _boardRenderer == null) return;
        
        var columns = System.Linq.Enumerable.Select(_activeTetromino.BaseTetrominos, t => (int)t.GridPosition.X);
        _boardRenderer.HighlightColumns(System.Linq.Enumerable.Distinct(columns));
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
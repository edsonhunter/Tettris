using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tettris.Controller.Shape;
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

    private CancellationTokenSource _dropCts;

    [Header("Scene References")]
    [SerializeField] private GameObject _background = null;
    [SerializeField] private Camera _camera = null;

    private IGameService GameService { get; set; }
    private int CurrentScore { get; set; }
    
    protected override void Loading(Action<bool> loaded)
    {
        GameService = GetService<IGameService>();
        GameService.CreateNewBoard(SceneData.Altura, SceneData.Largura);
        
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
        _inputHandler.OnMoveLeft += () => GameService.Move(Vector3.left);
        _inputHandler.OnMoveRight += () => GameService.Move(Vector3.right);
        _inputHandler.OnRotate += () => GameService.Rotate(Quaternion.Euler(0, 0, 90f));
        _inputHandler.OnFastDrop += () => _dropCts?.Cancel();
    private void InputHandler_OnFastDrop()
    {
        GameService.HardDrop();
        _dropCts?.Cancel();
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveLeft -= () => GameService.Move(Vector3.left);
            _inputHandler.OnMoveRight -= () => GameService.Move(Vector3.right);
            _inputHandler.OnRotate -= () => GameService.Rotate(Quaternion.Euler(0, 0, 90f));
            _inputHandler.OnFastDrop -= () => _dropCts?.Cancel();
        }
    }

    protected override void Loop()
    {
        // Loop is now empty as Input is handled by TetrisInputHandler
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Services.Interface;
using System.Numerics;

public class GameService : IGameService
{
    public IBoard Board { get; private set; }
    public ITetromino Tetromino { get; private set; }
    public float CurrentLevel { get; private set; }
    public bool Running { get; private set; }
    private CancellationTokenSource _tickCts;
    private bool _isFastDropping;
    
    public bool IsFastDropping 
    { 
        get => _isFastDropping; 
        set 
        {
            _isFastDropping = value;
            if (_isFastDropping) 
            {
                _tickCts?.Cancel();
            }
        } 
    }

    public event Action<ITetromino> OnTetrominoSpawned;
    public event Action OnPieceLanded;
    public event Action<int> OnLinesCleared;
    public event Action<float> OnLevelChanged;
    public event Action OnGameOver;
    
    public GameService()
    {
    }

    public void CreateNewBoard(int linhas, int colunas)
    {
        CurrentLevel = 0;
        Running = true;
        Tetromino = null;
        Board = Factory.CreateBoard(linhas, colunas);
    }

    public ITetromino NextRound()
    {
        CurrentLevel++;
        Tetromino = Factory.CreateTetromino();
        
        Vector2 spawnPosition = new Vector2((int)Math.Floor(Board.Colunas / 2f), Board.Linhas - 1);
        Tetromino.Move(spawnPosition);
        IsFastDropping = false;
        return Tetromino;
    }

    public void Move(Vector2 direction)
    {
        if (Tetromino == null)
        {
            return;
        }

        var temporaryPos = Factory.Move(Tetromino.BaseTetrominos, direction);
        if (!Board.Move(temporaryPos))
        {
            return;
        }

        Tetromino.Move(direction);
    }

    public void Rotate(float angleDegrees)
    {
        if (Tetromino == null || Tetromino.TetrominoType == TetrominoType.O)
        {
            return;
        }

        var temporaryPos = Factory.Rotate(Tetromino.BaseTetrominos, angleDegrees);
        if (!Board.Rotate(temporaryPos))
        {
            return;
        }

        Tetromino.Rotate(temporaryPos.Select(x => x.GridPosition).ToList());
    }

    public bool NextTurno()
    {
        var temporaryPos = Factory.Move(Tetromino.BaseTetrominos, new Vector2(0, -1));
        if (Board.Move(temporaryPos))
        {
            Tetromino.Move(new Vector2(0, -1));
            return true;
        }

        if (!Board.FinishTurno(Tetromino.BaseTetrominos))
        {
            Running = false;
        }
        
        
        IsFastDropping = false;
        Tetromino = null;

        return false;
    }

    public IList<int> CompleteLine()
    {
        return Board.CompleteLine();
    }

    public float Speed()
    {
        float baseSpeed = 5f / (CurrentLevel + 1);
        return IsFastDropping ? baseSpeed * 0.1f : baseSpeed;
    }

    public bool GameOver()
    {
        return !Running;
    }

    public async Task StartGameAsync(CancellationToken token)
    {
        Running = true;
        
        var tetromino = NextRound();
        OnTetrominoSpawned?.Invoke(tetromino);
        OnLevelChanged?.Invoke(CurrentLevel);

        _tickCts = CancellationTokenSource.CreateLinkedTokenSource(token);

        while (Running && !token.IsCancellationRequested)
        {
            try
            {
                await Task.Delay((int)(Speed() * 1000), _tickCts.Token);
            }
            catch (TaskCanceledException)
            {
                if (token.IsCancellationRequested) break;
                
                _tickCts?.Dispose();
                _tickCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            }

            if (token.IsCancellationRequested) break;

            if (!NextTurno())
            {
                OnPieceLanded?.Invoke();

                var completedLines = CompleteLine();
                if (completedLines.Count > 0)
                {
                    int score = completedLines.Count * 100;
                    OnLinesCleared?.Invoke(score);
                    
                    try
                    {
                        await Task.Delay(500, token);
                    }
                    catch (TaskCanceledException) { break; }
                }

                if (Running)
                {
                    var nextTetromino = NextRound();
                    OnTetrominoSpawned?.Invoke(nextTetromino);
                    OnLevelChanged?.Invoke(CurrentLevel);
                }
            }
        }

        if (!Running && !token.IsCancellationRequested)
        {
            OnGameOver?.Invoke();
        }

        _tickCts?.Dispose();
        _tickCts = null;
    }
}
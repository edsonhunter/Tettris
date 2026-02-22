using System.Collections.Generic;
using System.Linq;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Services.Interface;
using UnityEngine;

public class GameService : IGameService
{
    public IBoard Board { get; private set; }
    public ITetromino Tetromino { get; private set; }
    public float CurrentLevel { get; private set; }
    public bool Running { get; private set; }
    
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
        
        Vector2 spawnPosition = new Vector2(Mathf.FloorToInt(Board.Colunas / 2), Board.Linhas - 1);
        Tetromino.Move(spawnPosition);
        
        return Tetromino;
    }

    public void Move(Vector3 direction)
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

    public void Rotate(Quaternion direction)
    {
        if (Tetromino == null)
        {
            return;
        }

        var temporaryPos = Factory.Rotate(Tetromino.BaseTetrominos, direction);
        if (!Board.Rotate(temporaryPos))
        {
            return;
        }

        Tetromino.Rotate(temporaryPos.Select(x => x.GridPosition).ToList());
    }

    public bool NextTurno()
    {
        var temporaryPos = Factory.Move(Tetromino.BaseTetrominos, Vector2.down);
        if (Board.Move(temporaryPos))
        {
            Tetromino.Move(Vector2.down);
            return true;
        }

        if (!Board.FinishTurno(Tetromino.BaseTetrominos))
        {
            Running = false;
        }
        
        return false;
    }

    public IList<int> CompleteLine()
    {
        return Board.CompleteLine();
    }

    public float Speed()
    {
        return 5 / (CurrentLevel + 1);
    }

    public bool GameOver()
    {
        return !Running;
    }
}
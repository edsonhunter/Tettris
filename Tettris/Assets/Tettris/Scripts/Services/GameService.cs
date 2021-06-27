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
        Running = true;
        Board = Factory.CreateBoard(linhas, colunas);
    }

    public ITetromino StartNewTetromino()
    {
        CurrentLevel++;
        Tetromino = Factory.CreateTetromino();
        return Tetromino;
    }

    public void Move(Vector3 newPos)
    {
        if (Tetromino == null)
        {
            return;
        }

        var temporaryPos = Factory.Move(Tetromino.BaseTetrominos, newPos);
        if (!Board.Move(temporaryPos))
        {
            return;
        }

        Tetromino.Move(newPos);
    }

    public void Rotate(Quaternion newPos)
    {
        if (Tetromino == null)
        {
            return;
        }

        var temporaryPos = Factory.Rotate(Tetromino.BaseTetrominos, newPos);
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

        Board.FinishTurno(Tetromino.BaseTetrominos);
        return false;
    }

    public IList<int> CompleteLine()
    {
        return Board.CompleteLine();
    }

    public bool GameOver()
    {
        return false;
    }
}
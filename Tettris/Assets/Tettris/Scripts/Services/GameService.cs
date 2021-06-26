using System;
using System.Collections.Generic;
using System.Linq;
using Tettris.Domain.Board;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Domain.Tetronimo;
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
        CreateNewBoard();
    }

    public void CreateNewBoard()
    {
        Running = true;
        Board = CreateBoard(40, 20);
    }

    public ITetromino StartNewTetromino()
    {
        Tetromino = CreateTetromino();
        Board.StartNewTetromino(Tetromino.BaseTetrominos);
        CurrentLevel++;
        return Tetromino;
    }

    public void Turno()
    {
        Move(Vector3.down);
    }
    
    public void Move(Vector3 newPos)
    {
        if (Board.Move(Move(Tetromino.BaseTetrominos, newPos)))
        {
            Board.ClearOldState(Tetromino.BaseTetrominos);
            Tetromino.Move(newPos);
        }
    }

    public void Rotate(Quaternion newPos)
    {
        var temporaryPos = Rotate(Tetromino.BaseTetrominos, newPos);
        if(Board.Rotate(temporaryPos))
        {
            Board.ClearOldState(Tetromino.BaseTetrominos);
            Tetromino.Rotate(temporaryPos.Select(x => x.GridPosition).ToList());
        }
    }
    
    private IBoard CreateBoard(int linha, int coluna)
    {
        ITile[,] tiles = new ITile[linha, coluna];

        for (int x = 0; x < linha; x++)
        {
            for (int y = 0; y < coluna; y++)
            {
                tiles[x, y] = new Tile(new Vector2(x, y));
            }
        }

        return new Board(linha, coluna, tiles);
    }
    
    private ITetromino CreateTetromino()
    {
        IList<IBaseTetromino> baseTetrominos = new List<IBaseTetromino>();
        Guid id = Guid.NewGuid();
        for (int baseIdx = 0; baseIdx < 4; baseIdx++)
        {
            baseTetrominos.Add(new BaseTetromino(id, Vector2.zero));
        }
        return new Tetronimo(id, baseTetrominos);
    }

    private static IList<IBaseTetromino> Move(IList<IBaseTetromino> currenTetrominos, Vector2 newPos)
    {
        return currenTetrominos.Select(baseTetromino =>
                new BaseTetromino(baseTetromino.TetronimoId, baseTetromino.GridPosition + newPos))
            .Cast<IBaseTetromino>().ToList();
    }
    
    private static IList<IBaseTetromino> Rotate(IList<IBaseTetromino> currenTetrominos, Quaternion newPos)
    {
        var rotatedTetromino = currenTetrominos.Select(baseTetromino =>
                new BaseTetromino(baseTetromino.TetronimoId, baseTetromino.GridPosition))
            .Cast<IBaseTetromino>().ToList();

        var pivot = currenTetrominos.First().GridPosition;
        foreach (IBaseTetromino baseTetromino in rotatedTetromino)
        {
            baseTetromino.Rotate(pivot, newPos);
        }

        return rotatedTetromino;
    }
}

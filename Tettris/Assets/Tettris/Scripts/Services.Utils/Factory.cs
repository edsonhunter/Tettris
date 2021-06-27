using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tettris.Domain.Board;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Domain.Tetronimo;
using UnityEngine;

public class Factory
{
    public static IBoard CreateBoard(int linha, int coluna)
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
    
    public static ITetromino CreateTetromino()
    {
        IList<IBaseTetromino> baseTetrominos = new List<IBaseTetromino>();
        Guid id = Guid.NewGuid();
        for (int baseIdx = 0; baseIdx < 4; baseIdx++)
        {
            baseTetrominos.Add(new BaseTetromino(id, Vector2.zero));
        }
        return new Tetronimo(id, baseTetrominos);
    }

    public static IList<IBaseTetromino> Move(IList<IBaseTetromino> currenTetrominos, Vector2 newPos)
    {
        return currenTetrominos.Select(baseTetromino =>
                new BaseTetromino(baseTetromino.TetronimoId, baseTetromino.GridPosition + newPos))
            .Cast<IBaseTetromino>().ToList();
    }
    
    public static IList<IBaseTetromino> Rotate(IList<IBaseTetromino> currenTetrominos, Quaternion newPos)
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

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
    
    public static ITetromino CreateTetromino(TetrominoType? type = null)
    {
        if (type == null)
        {
            var values = Enum.GetValues(typeof(TetrominoType));
            type = (TetrominoType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        Vector2[] positions;
        switch (type.Value)
        {
            case TetrominoType.I: positions = new[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(2, 0) }; break;
            case TetrominoType.J: positions = new[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1) }; break;
            case TetrominoType.L: positions = new[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(1, 1) }; break;
            case TetrominoType.O: positions = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) }; break;
            case TetrominoType.S: positions = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 1) }; break;
            case TetrominoType.T: positions = new[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1) }; break;
            case TetrominoType.Z: positions = new[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 1) }; break;
            default: positions = new[] { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(2, 0) }; break;
        }

        Guid id = Guid.NewGuid();
        IList<IBaseTetromino> baseTetrominos = positions.Select(pos => new BaseTetromino(id, pos)).Cast<IBaseTetromino>().ToList();
        return new Tetronimo(id, type.Value, baseTetrominos);
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

        float sumX = currenTetrominos.Sum(t => t.GridPosition.x);
        float sumY = currenTetrominos.Sum(t => t.GridPosition.y);
        int count = currenTetrominos.Count;
        Vector2 pivot = new Vector2(Mathf.Round(sumX / count), Mathf.Round(sumY / count));
        
        foreach (IBaseTetromino baseTetromino in rotatedTetromino)
        {
            baseTetromino.Rotate(pivot, newPos);
        }

        return rotatedTetromino;
    }
}

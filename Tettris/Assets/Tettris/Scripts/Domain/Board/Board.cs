using System;
using System.Collections;
using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using System.Linq;
using System.Text;
using Tettris.Domain.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Board
{
    public class Board : IBoard
    {
        public int Colunas { get; set; }
        public int Linhas { get; set; }
        public ITile[,] Tiles { get; private set; }

        public Board(int linhas, int colunas, ITile[,] tiles)
        {
            Linhas = linhas;
            Colunas = colunas;
            Tiles = tiles;
        }

        public void StartNewTetromino(IList<IBaseTetromino> startPosition)
        {
            Move(startPosition);
        }

        public bool Move(IList<IBaseTetromino> moveTetrominos)
        {
            var moved = false;
            StringBuilder sb = new StringBuilder();
            foreach (var movedTetromino in moveTetrominos)
            {
                var linhaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.y); //O Movimento lateral é baseado em X mas andamos nas colunas do vetor
                var colunaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.x); //O Movimento vertical é baseado em Y mas andamos nas linhas do vetor

                if (linhaIdx >= Linhas || colunaIdx >= Colunas || linhaIdx < 0 || colunaIdx < 0)
                {
                    moved = false;
                    break;
                }

                var tile = Tiles[linhaIdx, colunaIdx];
                if (!tile.OccupySlot(movedTetromino))
                {
                    moved = false;
                    break;
                }

                sb.Append($"Tile x: {tile.Position.x}, y: {tile.Position.y} | ");
                moved = true;
            }
            Debug.Log(sb.ToString());
            return moved;
        }

        public bool Rotate(IList<IBaseTetromino> moveTetrominos)
        {
            var moved = false;
            StringBuilder sb = new StringBuilder();
            for (int cubeIdx = 0; cubeIdx < moveTetrominos.Count; cubeIdx++)
            {
                var movedTetromino = moveTetrominos[cubeIdx];
                var linhaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.y); //O Movimento lateral é baseado em X mas andamos nas colunas do vetor
                var colunaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.x); //O Movimento vertical é baseado em Y mas andamos nas linhas do vetor

                if (linhaIdx >= Linhas || colunaIdx >= Colunas || linhaIdx < 0 || colunaIdx < 0)
                {
                    moved = false;
                    break;
                }

                var tile = Tiles[linhaIdx, colunaIdx];
                if (!tile.OccupySlot(movedTetromino))
                {
                    moved = false;
                    break;
                }
                sb.Append($"Tile x: {tile.Position.x}, y: {tile.Position.y} | ");
                moved = true;
            }
            Debug.Log(sb.ToString());
            return moved;
        }

        public void ClearOldState(IList<IBaseTetromino> baseTetrominosToClear, IList<IBaseTetromino> newPositionBaseTetrominos)
        {
            for (int posIdx = 0; posIdx < baseTetrominosToClear.Count; posIdx++)
            {
                var newTile = newPositionBaseTetrominos[posIdx];
                var oldTile = baseTetrominosToClear[posIdx];
                //Se o tile a ser deletado esta na mesma posicao da nova lista
                //Significa que nao podemos deletado
                if (newTile.GridPosition.x == oldTile.GridPosition.x &&
                    newTile.GridPosition.y == oldTile.GridPosition.y)
                {
                    continue;
                }
                
                var linhaIdx = Mathf.FloorToInt(oldTile.GridPosition.y); //O Movimento lateral é baseado em X mas andamos nas colunas do vetor
                var colunaIdx = Mathf.FloorToInt(oldTile.GridPosition.x); //O Movimento vertical é baseado em Y mas andamos nas linhas do vetor
                var tile = Tiles[linhaIdx, colunaIdx];
                tile.ReleaseSlot();
            }
        }

        public bool CompleteLine()
        {
            var completed = false;

            IList<ITile> tilesToClear = new List<ITile>();
            for (int x = 0; x < Linhas; x++)
            {
                for (int y = 0; y < Colunas; y++)
                {
                    if (!Tiles[x, y].Occupy)
                    {
                        break;
                    }

                    tilesToClear.Add(Tiles[x, y]);
                }

                if (tilesToClear.Count <= 0)
                {
                    continue;
                }

                if (tilesToClear.Count < Colunas)
                {
                    tilesToClear.Clear();
                }
                else
                {
                    foreach (ITile tile in tilesToClear)
                    {
                        completed = true;
                        tile.ReleaseSlot();
                    }

                    tilesToClear.Clear();
                }
            }

            if (!completed)
            {
                return false;
            }

            IList<IBaseTetromino> tetrominosToMove = new List<IBaseTetromino>();
            foreach (ITile tile in Tiles)
            {
                if (tile.Occupy)
                {
                    tetrominosToMove.Add(new BaseTetromino(tile.CurrentTetromino.TetronimoId, tile.CurrentTetromino.Move(Vector2.down)));
                }
            }

            return completed;
        }
    }
}
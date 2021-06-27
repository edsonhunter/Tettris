using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Domain.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Board
{
    public class Board : IBoard
    {
        public int Colunas { get; private set; }
        public int Linhas { get; private set; }
        public ITile[,] Tiles { get; private set; }

        public Board(int linhas, int colunas, ITile[,] tiles)
        {
            Linhas = linhas;
            Colunas = colunas;
            Tiles = tiles;
        }

        public bool StartNewTetromino(IList<IBaseTetromino> startPosition)
        {
            return Move(startPosition);
        }

        public bool Move(IList<IBaseTetromino> moveTetrominos)
        {
            var moved = false;
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
                if (!tile.CheckSlot(movedTetromino))
                {
                    moved = false;
                    break;
                }

                moved = true;
            }

            return moved;
        }

        public bool Rotate(IList<IBaseTetromino> moveTetrominos)
        {
            var moved = false;
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
                if (!tile.CheckSlot(movedTetromino))
                {
                    moved = false;
                    break;
                }

                moved = true;
            }

            return moved;
        }

        public void FinishTurno(IList<IBaseTetromino> tetrominoBaseTetrominos)
        {
            foreach (var movedTetromino in tetrominoBaseTetrominos)
            {
                var linhaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.y); //O Movimento lateral é baseado em X mas andamos nas colunas do vetor
                var colunaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.x); //O Movimento vertical é baseado em Y mas andamos nas linhas do vetor
                var tile = Tiles[linhaIdx, colunaIdx];
                tile.OccupySlot(movedTetromino);
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
            
            return completed;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
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

        public bool Move(IList<IBaseTetromino> moveTetrominos)
        {
            var moved = false;
            foreach (var movedTetromino in moveTetrominos)
            {
                var linhaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.y); //O Movimento lateral é baseado em X mas andamos nas colunas do vetor
                var colunaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.x); //O Movimento vertical é baseado em Y mas andamos nas linhas do vetor

                if (colunaIdx < 0 && colunaIdx > -1)
                {
                    colunaIdx = 0;
                }
                
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

        public bool FinishTurno(IList<IBaseTetromino> tetrominoBaseTetrominos)
        {
            foreach (var movedTetromino in tetrominoBaseTetrominos)
            {
                var linhaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.y); //O Movimento lateral é baseado em X mas andamos nas colunas do vetor
                var colunaIdx = Mathf.FloorToInt(movedTetromino.GridPosition.x); //O Movimento vertical é baseado em Y mas andamos nas linhas do vetor
                if (!Tiles[linhaIdx, colunaIdx].OccupySlot(movedTetromino))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckLines(int line)
        {
            for (int column = 0; column < Colunas; column++)
            {
                if (!Tiles[line, column].Occupy)
                    return false;
            }
            return true;
        }

        public IList<int> CompleteLine()
        {
            IList<int> lines = new List<int>();
            for (int line = 0; line < Linhas; line++)
            {
                if (CheckLines(line))
                {
                    lines.Add(line);
                    ClearRow(line);
                    RowDown(line);
                }
            }

            return lines;
        }

        private void ClearRow(int line)
        {
            for (int column = 0; column < Colunas; column++)
            {
                if (Tiles[line, column].Occupy)
                {
                    Tiles[line, column].ReleaseSlot();
                }
            }
        }

        private void RowDown(int lineDeleted)
        {
            for (int line = lineDeleted; line < Linhas; line++)
            {
                for (int column = 0; column < Colunas; column++)
                {
                    if (!Tiles[line, column].Occupy)
                    {
                        continue;
                    }
                    Tiles[line - 1, column].OccupySlot(Tiles[line, column].CurrentTetromino);
                    Tiles[line, column].ReleaseSlot();
                }
            }
        }
        
        // public IList<int> CompleteLine()
        // {
        //     IList<int> lines = new List<int>();
        //     IList<ITile> tilesToClear = new List<ITile>();
        //     for (int x = 0; x < Linhas; x++)
        //     {
        //         for (int y = 0; y < Colunas; y++)
        //         {
        //             if (!Tiles[x, y].Occupy)
        //             {
        //                 break;
        //             }
        //
        //             tilesToClear.Add(Tiles[x, y]);
        //         }
        //
        //         if (tilesToClear.Count <= 0)
        //         {
        //             continue;
        //         }
        //
        //         if (tilesToClear.Count < Colunas)
        //         {
        //             tilesToClear.Clear();
        //         }
        //         else //limpa toda a linha antes de olhar a proxima
        //         {
        //             lines.Add(x);
        //             foreach (ITile tile in tilesToClear)
        //             {
        //                 tile.ReleaseSlot();
        //             }
        //             tilesToClear.Clear();
        //         }
        //     }
        //
        //     return lines;
        // }
    }
}
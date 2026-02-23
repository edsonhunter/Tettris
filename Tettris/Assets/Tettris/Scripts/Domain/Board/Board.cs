using System;
using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using System.Numerics;

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
                var linhaIdx = (int)Math.Floor(movedTetromino.GridPosition.Y); 
                var colunaIdx = (int)Math.Floor(movedTetromino.GridPosition.X);

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
                var linhaIdx = (int)Math.Floor(movedTetromino.GridPosition.Y); 
                var colunaIdx = (int)Math.Floor(movedTetromino.GridPosition.X);

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
                var linhaIdx = (int)Math.Floor(movedTetromino.GridPosition.Y);
                var colunaIdx = (int)Math.Floor(movedTetromino.GridPosition.X);
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
                if (!Tiles[line, column].Occupied)
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
                    line--;
                }
            }

            return lines;
        }

        private void ClearRow(int line)
        {
            for (int column = 0; column < Colunas; column++)
            {
                if (Tiles[line, column].Occupied)
                {
                    var tetromino = Tiles[line, column].CurrentTetromino;
                    tetromino.Destroy();
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
                    if (!Tiles[line, column].Occupied)
                    {
                        continue;
                    }
                    var t = Tiles[line, column].CurrentTetromino;
                    t.Move(new Vector2(0, -1));
                    Tiles[line - 1, column].OccupySlot(t);
                    Tiles[line, column].ReleaseSlot();
                }
            }
        }
    }
}
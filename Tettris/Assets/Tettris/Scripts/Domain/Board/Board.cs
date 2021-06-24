using System;
using System.Collections;
using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using System.Linq;
using Tettris.Domain.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Board
{
    public class Board : IBoard
    {
        public int Altura { get; set; }
        public int Largura { get; set; }
        public ITile[,] Tiles { get; private set; }
        private ITetromino CurrentTetromino { get; set; }

        public Board(int altura, int largura, ITile[,] tiles)
        {
            Altura = altura;
            Largura = largura;
            Tiles = tiles;
        }

        public void StartNewTetromino(ITetromino newTetromino)
        {
            CurrentTetromino = newTetromino;
        }

        public bool Move(IList<IBaseTetromino> cubesTetromino)
        {
            var moved = false;
            foreach (var cubes in cubesTetromino)
            {
                if (cubes.GridPosition.x >= Altura || cubes.GridPosition.y >= Largura)
                {
                    moved = false;
                    break;
                }

                var tile = Tiles[(int) Math.Round(cubes.GridPosition.x), (int) Math.Round(cubes.GridPosition.y)];
                if (!tile.OccupySlot(cubes))
                {
                    moved = false;
                    break;
                }

                moved = true;
            }
            return moved;
        }

        public void ClearOldState(ITetromino tetromino)
        {
            foreach (var cubes in CurrentTetromino.BaseTetrominos)
            {
                Tiles[(int) Math.Round(cubes.GridPosition.x), (int) Math.Round(cubes.GridPosition.y)].ReleaseSlot();
            }

            CurrentTetromino = tetromino;
        }

        public bool CompleteLine()
        {
            var completed = false;

            IList<ITile> tilesToClear = new List<ITile>();
            for (int x = 0;
                x < Altura;
                x++)
            {
                for (int y = 0; y < Largura; y++)
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

                if (tilesToClear.Count < Largura)
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
                    tetrominosToMove.Add(new BaseTetromino(tile.CurrentTetromino.TetronimoId, tile.CurrentTetromino.Move(Vector2.down), tile.CurrentTetromino.Color));
                }
            }

            return completed;
        }
    }
}
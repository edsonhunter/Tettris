using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tettris.Domain.Board;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Domain.Tetronimo;
using UnityEngine;

namespace DefaultNamespace
{
    public class TetronimoTest
    {
        private IBoard CreateBoard(int altura, int largura)
        {
            ITile[,] tiles = new ITile[altura, largura];

            for (int x = 0; x < altura; x++)
            {
                for (int y = 0; y < largura; y++)
                {
                    tiles[x, y] = new Tile(new Vector2(x, y));
                }
            }

            return new Board(altura, largura, tiles);
        }

        private ITetromino CreateHouseTetromino()
        {
            IList<IBaseTetromino> baseTetrominos = new List<IBaseTetromino>();
            Guid id = Guid.NewGuid();

            //House
            var cube01 = new Vector2(1, 0);
            var b1 = new BaseTetromino(id, cube01);
            baseTetrominos.Add(b1);
            var cube02 = new Vector2(1, 1);
            var b2 = new BaseTetromino(id, cube02);
            baseTetrominos.Add(b2);
            var cube03 = new Vector2(0, 0);
            var b3 = new BaseTetromino(id, cube03);
            baseTetrominos.Add(b3);
            var cube04 = new Vector2(2, 0);
            var b4 = new BaseTetromino(id, cube04);
            baseTetrominos.Add(b4);
            return new Tetronimo(id, baseTetrominos);
        }

        private ITetromino CreateLineTetromino()
        {
            IList<IBaseTetromino> baseTetrominos = new List<IBaseTetromino>();
            Guid id = Guid.NewGuid();

            //Line
            var cube01 = new Vector2(0, 0);
            var b1 = new BaseTetromino(id, cube01);
            baseTetrominos.Add(b1);
            var cube02 = new Vector2(0, 1);
            var b2 = new BaseTetromino(id, cube02);
            baseTetrominos.Add(b2);
            var cube03 = new Vector2(0, 2);
            var b3 = new BaseTetromino(id, cube03);
            baseTetrominos.Add(b3);
            var cube04 = new Vector2(0, 3);
            var b4 = new BaseTetromino(id, cube04);
            baseTetrominos.Add(b4);
            return new Tetronimo(id, baseTetrominos);
        }

        [TestCase(10, 20)]
        public void GamePlay(int altura, int largura)
        {
            var board = CreateBoard(altura, largura);
            var tetromino = CreateLineTetromino();
            var oldTetromino = CreateLineTetromino();
            board.StartNewTetromino(Move(tetromino.BaseTetrominos, Vector2.zero));
            var newPos = new Vector2(1, 0);
            if (board.Move(Move(tetromino.BaseTetrominos, newPos)))
            {
                board.ClearOldState(Move(oldTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
                oldTetromino.Move(newPos);
            }

            Assert.True(board.Tiles[1, 1].Occupy);
        }

        [TestCase()]
        public void GameLoop()
        {
            var chegouAoFim = false;
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            var oldTetromino = CreateLineTetromino();
            board.StartNewTetromino(Move(tetromino.BaseTetrominos, Vector2.zero));
            for (int i = 0; i < 10; i++)
            {
                var newPos = new Vector2(1, 0);
                if (!board.Move(Move(tetromino.BaseTetrominos, newPos)))
                {
                    continue;
                }

                board.ClearOldState(Move(oldTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
                oldTetromino.Move(newPos);
            }

            Assert.True(board.CompleteLine());
        }

        [Test]
        public void Rotate()
        {
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            var oldTetromino = CreateLineTetromino();
            board.StartNewTetromino(Move(tetromino.BaseTetrominos, Vector2.zero));

            for (int i = 0; i < 5; i++)
            {
                var newPos = new Vector2(1, 0);
                if (!board.Move(Move(tetromino.BaseTetrominos, newPos)))
                {
                    board.ClearOldState(Move(oldTetromino.BaseTetrominos, Vector2.zero));
                    continue;
                }

                board.ClearOldState(Move(oldTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
                oldTetromino.Move(newPos);
            }
            
            //ROTATE

            for (int i = 0; i < 5; i++)
            {
                var newPos = new Vector2(1, 0);
                if (!board.Move(Move(tetromino.BaseTetrominos, newPos)))
                {
                    board.ClearOldState(Move(oldTetromino.BaseTetrominos, Vector2.zero));
                    continue;
                }

                board.ClearOldState(Move(oldTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
                oldTetromino.Move(newPos);
            }

            Assert.False(board.CompleteLine());
        }

        private IList<IBaseTetromino> Move(IList<IBaseTetromino> currenTetrominos, Vector2 newPos)
        {
            return currenTetrominos.Select(baseTetromino =>
                    new BaseTetromino(baseTetromino.TetronimoId, baseTetromino.GridPosition + newPos))
                .Cast<IBaseTetromino>().ToList();
        }
    }
}
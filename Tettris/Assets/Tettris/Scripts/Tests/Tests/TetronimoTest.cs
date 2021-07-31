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
            var cube02 = new Vector2(1, 0);
            var b2 = new BaseTetromino(id, cube02);
            baseTetrominos.Add(b2);
            var cube03 = new Vector2(2, 0);
            var b3 = new BaseTetromino(id, cube03);
            baseTetrominos.Add(b3);
            var cube04 = new Vector2(3, 0);
            var b4 = new BaseTetromino(id, cube04);
            baseTetrominos.Add(b4);
            return new Tetronimo(id, baseTetrominos);
        }

        [Test]
        public void GamePlay()
        {
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            var direction = Vector2.up;
            if (board.Move(Factory.Move(tetromino.BaseTetrominos, direction)))
            {
                tetromino.Move(direction);
            }

            board.FinishTurno(tetromino.BaseTetrominos);
            Assert.True(board.Tiles[1, 1].Occupied);
        }

        [Test]
        public void GameLoop()
        {
            var chegouAoFim = false;
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            for (int i = 0; i < 10; i++)
            {
                var direction = Vector2.up;
                if (!board.Move(Factory.Move(tetromino.BaseTetrominos, direction)))
                {
                    continue;
                }

                tetromino.Move(direction);
            }
            board.FinishTurno(tetromino.BaseTetrominos);
            Assert.True(board.CompleteLine().Count > 0);
        }

        [TestCase(true, TestName =  "Rotate dont complete line")]
        [TestCase(false, TestName =  "Dont rotate complete line")]
        public void Rotate(bool rotate)
        {
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            for (int i = 0; i < 5; i++)
            {
                var direction = Vector2.up;
                if (!board.Move(Factory.Move(tetromino.BaseTetrominos, direction)))
                {
                    continue;
                }

                tetromino.Move(direction);
            }

            if (rotate)
            {
                var temporaryPos = Factory.Rotate(tetromino.BaseTetrominos, Quaternion.Euler(0, 0, 90f));
                if (board.Rotate(temporaryPos))
                {
                    tetromino.Rotate(temporaryPos.Select(x => x.GridPosition).ToList());
                }
            }

            for (int i = 0; i < 5; i++)
            {
                var newPos = Vector2.up;
                if (!board.Move(Factory.Move(tetromino.BaseTetrominos, newPos)))
                {
                    continue;
                }

                tetromino.Move(newPos);
            }

            board.FinishTurno(tetromino.BaseTetrominos);
            Assert.AreEqual(!rotate, board.CompleteLine().Count > 0);
        }
    }
}
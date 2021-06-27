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
            var futureTetromino = CreateLineTetromino();
            board.StartNewTetromino(GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero));
            var newPos = new Vector2(1, 0);
            if (board.Move(GameService.Move(tetromino.BaseTetrominos, newPos)))
            {
                board.ClearOldState(tetromino.BaseTetrominos, GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
            }

            Assert.True(board.Tiles[1, 1].Occupy);
        }

        [TestCase()]
        public void GameLoop()
        {
            var chegouAoFim = false;
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            var futureTetromino = CreateLineTetromino();
            board.StartNewTetromino(GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero));
            for (int i = 0; i < 10; i++)
            {
                var newPos = new Vector2(1, 0);
                if (!board.Move(GameService.Move(tetromino.BaseTetrominos, newPos)))
                {
                    continue;
                }

                board.ClearOldState(tetromino.BaseTetrominos, GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
            }

            Assert.True(board.CompleteLine());
        }

        [TestCase(true, TestName =  "Rotate dont complete line")]
        [TestCase(false, TestName =  "Dont rotate complete line")]
        public void Rotate(bool rotate)
        {
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            var futureTetromino = CreateLineTetromino();
            board.StartNewTetromino(GameService.Move(tetromino.BaseTetrominos, Vector2.zero));

            for (int i = 0; i < 5; i++)
            {
                var newPos = new Vector2(1, 0);
                if (!board.Move(GameService.Move(futureTetromino.BaseTetrominos, newPos)))
                {
                    board.ClearOldState(GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero), tetromino.BaseTetrominos);
                    continue;
                }

                board.ClearOldState(tetromino.BaseTetrominos, GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero));
                tetromino.Move(newPos);
            }

            if (rotate)
            {
                var temporaryPos = GameService.Rotate(futureTetromino.BaseTetrominos, Quaternion.Euler(0, 0, 90f));
                if (board.Rotate(temporaryPos))
                {
                    board.ClearOldState(tetromino.BaseTetrominos, temporaryPos);
                    tetromino.Rotate(temporaryPos.Select(x => x.GridPosition).ToList());
                }
                else
                {
                    board.ClearOldState(GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero), tetromino.BaseTetrominos);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                var newPos = new Vector2(1, 0);
                if (!board.Move(GameService.Move(futureTetromino.BaseTetrominos, newPos)))
                {
                    board.ClearOldState(GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero), tetromino.BaseTetrominos);
                    continue;
                }

                board.ClearOldState(GameService.Move(futureTetromino.BaseTetrominos, Vector2.zero), tetromino.BaseTetrominos);
                tetromino.Move(newPos);
            }

            Assert.AreEqual(!rotate, board.CompleteLine());
        }
    }
}
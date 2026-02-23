using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tettris.Domain.Board;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Domain.Tetronimo;
using System.Numerics;

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
            return new Tetronimo(id, TetrominoType.T, baseTetrominos);
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
            return new Tetronimo(id, TetrominoType.L, baseTetrominos);
        }

        [Test]
        public void GamePlay()
        {
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            var direction = new Vector2(0, 1);
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
            var board = CreateBoard(10, 4);
            var tetromino = CreateLineTetromino();
            for (int i = 0; i < 10; i++)
            {
                var direction = new Vector2(0, 1);
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
                var direction = new Vector2(0, 1);
                if (!board.Move(Factory.Move(tetromino.BaseTetrominos, direction)))
                {
                    continue;
                }

                tetromino.Move(direction);
            }

            if (rotate)
            {
                var temporaryPos = Factory.Rotate(tetromino.BaseTetrominos, 90f);
                if (board.Rotate(temporaryPos))
                {
                    tetromino.Rotate(temporaryPos.Select(x => x.GridPosition).ToList());
                }
            }

            for (int i = 0; i < 5; i++)
            {
                var newPos = new Vector2(0, 1);
                if (!board.Move(Factory.Move(tetromino.BaseTetrominos, newPos)))
                {
                    continue;
                }

                tetromino.Move(newPos);
            }

            board.FinishTurno(tetromino.BaseTetrominos);
            Assert.AreEqual(!rotate, board.CompleteLine().Count > 0);
        }

        [Test]
        public void CompleteMultipleLines()
        {
            var board = CreateBoard(10, 4);

            // Fill row 0 completely
            for (int i = 0; i < 4; i++)
                board.Tiles[0, i].OccupySlot(new BaseTetromino(Guid.NewGuid(), new Vector2(i, 0)));

            // Fill row 1 completely
            for (int i = 0; i < 4; i++)
                board.Tiles[1, i].OccupySlot(new BaseTetromino(Guid.NewGuid(), new Vector2(i, 1)));

            // Fill row 2 partially (this should drop down to row 0)
            board.Tiles[2, 0].OccupySlot(new BaseTetromino(Guid.NewGuid(), new Vector2(0, 2)));

            var clearedLines = board.CompleteLine();

            Assert.AreEqual(2, clearedLines.Count, "Should clear exactly 2 lines simultaneously");

            // Row 0 should now contain the block that was originally at row 2
            Assert.IsTrue(board.Tiles[0, 0].Occupied, "Block from row 2 should fall to row 0");
            Assert.IsFalse(board.Tiles[0, 1].Occupied, "Other columns in row 0 should be empty");

            // Row 1 and Row 2 should now be completely empty
            Assert.IsFalse(board.Tiles[1, 0].Occupied, "Row 1 should be empty after dropping");
            Assert.IsFalse(board.Tiles[2, 0].Occupied, "Row 2 should be empty after dropping");
        }
    }
}
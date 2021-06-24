using System;
using System.Collections.Generic;
using System.Text;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Tetronimo
{
    public class Tetronimo : ITetromino
    {
        public Guid TetronimoId { get; }
        public IList<IBaseTetromino> BaseTetrominos => _baseTetrominos.AsReadOnly();
        private List<IBaseTetromino> _baseTetrominos { get; set; }
        
        public Tetronimo(Guid tetronimoId, IList<IBaseTetromino> baseTetrominos)
        {
            TetronimoId = tetronimoId;
            _baseTetrominos = new List<IBaseTetromino>(baseTetrominos);
        }

        public IList<IBaseTetromino> Rotate()
        {
            foreach (var baseTetromino in _baseTetrominos)
            {
                baseTetromino.Rotate(Vector2.left);
            }

            return _baseTetrominos;
        }

        public IList<IBaseTetromino> Move(int x, int y)
        {
            foreach (var baseTetromino in _baseTetrominos)
            {
                baseTetromino.Move(new Vector2(baseTetromino.GridPosition.x + x, baseTetromino.GridPosition.y + y));
            }

            StringBuilder sb = new StringBuilder();
            foreach (IBaseTetromino baseTetromino in _baseTetrominos)
            {
                sb.Append(string.Format("x: {0}, y: {1} | ",baseTetromino.GridPosition.x, baseTetromino.GridPosition.y));
            }
            Debug.Log(sb.ToString());
            return _baseTetrominos;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Tetronimo
{
    public class Tetronimo : ITetromino
    {
        public Guid TetronimoId { get; }
        public IList<IBaseTetromino> BaseTetrominos => _baseTetrominos.AsReadOnly();
        public Vector2 GridPosition { get; private set; }
        private List<IBaseTetromino> _baseTetrominos { get; set; }
        private EventHandler<Vector3> _onMove { get; set; }
        private EventHandler<IList<Vector2>> _onRotate { get; set; }
        
        public event EventHandler<Vector3> OnMove
        {
            add => _onMove += value;
            remove => _onMove -= value;
        }
        public event EventHandler<IList<Vector2>> OnRotate
        {
            add => _onRotate += value;
            remove => _onRotate -= value;
        }

        public Tetronimo(Guid tetronimoId, IList<IBaseTetromino> baseTetrominos)
        {
            TetronimoId = tetronimoId;
            _baseTetrominos = new List<IBaseTetromino>(baseTetrominos);
        }

        public void StartPosition(List<Vector3> startPositions)
        {
            GridPosition = startPositions.First();
            
            for (int baseTetrominoIdx = 0; baseTetrominoIdx < _baseTetrominos.Count; baseTetrominoIdx++)
            {
                _baseTetrominos[baseTetrominoIdx].Move(startPositions[baseTetrominoIdx]);
            }
        }

        public void SetPosition(Vector2 transformPosition)
        {
            GridPosition = transformPosition;
        }

        public void Rotate(IList<Vector2> newPos)
        {
            StringBuilder sb = new StringBuilder();
            for (int posIdx = 0; posIdx < _baseTetrominos.Count; posIdx++)
            {
                _baseTetrominos[posIdx].SetPosition(newPos[posIdx]);
                sb.Append($"Tetronimo x:{newPos[posIdx].x}, y: {newPos[posIdx].y} | ");
            }
            Debug.Log(sb.ToString());
            _onRotate?.Invoke(this, newPos);
        }

        public void Move(Vector2 newPos)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var baseTetromino in _baseTetrominos)
            {
                sb.Append(" | Tetronimo pos:" + baseTetromino.Move(newPos));
            }

            Debug.Log(sb.ToString());
            _onMove?.Invoke(this, newPos);
        }
    }
}
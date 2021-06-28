using System;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Utils;
using UnityEngine;

namespace Tettris.Domain.Tetronimo
{
    public class BaseTetromino : IBaseTetromino
    { 
        public Guid TetronimoId { get; private set; }
        public Vector2 GridPosition { get; private set; }

        public BaseTetromino(Guid tetronimoId, Vector2 gridPosition)
        {
            TetronimoId = tetronimoId;
            GridPosition = gridPosition;
        }

        public void Rotate(Vector2 pivot, Quaternion newPos)
        {
            GridPosition = GridPosition.RotateAroundPivotVector2(pivot, newPos);
        }
        
        public Vector2 Move(Vector2 newPos)
        {
            GridPosition += newPos;
            return GridPosition;
        }

        public void SetPosition(Vector2 newPo)
        {
            GridPosition = newPo;
        }
    }
}
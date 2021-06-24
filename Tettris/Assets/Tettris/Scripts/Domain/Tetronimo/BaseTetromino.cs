using System;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Tetronimo
{
    public class BaseTetromino : IBaseTetromino
    { 
        public Guid TetronimoId { get; private set; }
        public Vector2 GridPosition { get; private set; }
        public int Color { get; private set; }

        public BaseTetromino(Guid tetronimoId, Vector2 gridPosition, int color)
        {
            TetronimoId = tetronimoId;
            GridPosition = gridPosition;
            Color = color;
        }

        public Vector2 Rotate(Vector2 rotateVector)
        {
            return GridPosition += rotateVector;
        }
        
        public Vector2 Move(Vector2 newPos)
        {
            GridPosition = newPos;
            return GridPosition;
        }
    }
}
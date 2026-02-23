using System;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.Utils;
using System.Numerics;

namespace Tettris.Domain.Tetronimo
{
    public class BaseTetromino : IBaseTetromino
    { 
        public Guid TetronimoId { get; private set; }
        public Vector2 GridPosition { get; private set; }

        public event EventHandler OnDestroyed;
        public event EventHandler<Vector2> OnPositionChanged;

        public BaseTetromino(Guid tetronimoId, Vector2 gridPosition)
        {
            TetronimoId = tetronimoId;
            GridPosition = gridPosition;
        }

        public void Destroy()
        {
            OnDestroyed?.Invoke(this, EventArgs.Empty);
        }

        public void Rotate(Vector2 pivot, float angleDegrees)
        {
            GridPosition = GridPosition.RotateAroundPivotVector2(pivot, angleDegrees);
            GridPosition = new Vector2((float)Math.Round(GridPosition.X), (float)Math.Round(GridPosition.Y));
            OnPositionChanged?.Invoke(this, GridPosition);
        }
        
        public Vector2 Move(Vector2 newPos)
        {
            GridPosition += newPos;
            OnPositionChanged?.Invoke(this, GridPosition);
            return GridPosition;
        }

        public void SetPosition(Vector2 newPo)
        {
            GridPosition = newPo;
            OnPositionChanged?.Invoke(this, GridPosition);
        }
    }
}
using System;
using UnityEngine;

namespace Tettris.Domain.Interface.Tetronimo
{
    public interface IBaseTetromino
    {
        Guid TetronimoId { get; }
        Vector2 GridPosition { get; }
        
        event EventHandler OnDestroyed;
        event EventHandler<Vector2> OnPositionChanged;
        
        void Destroy();
        void Rotate(Vector2 pivot, Quaternion newPos);
        Vector2 Move(Vector2 newPos);
        void SetPosition(Vector2 newPo);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tettris.Domain.Interface.Tetronimo
{
    public interface ITetromino
    {
        Guid TetronimoId { get; }
        IList<IBaseTetromino> BaseTetrominos { get; }
        Vector2 GridPosition { get; }
        event EventHandler<Vector3> OnMove;
        event EventHandler<IList<Vector2>> OnRotate;

        void StartPosition(List<Vector3> startPositions);
        void SetPosition(Vector2 transformPosition);
        void Move(Vector2 newPos);
        void Rotate(IList<Vector2> newPos);
    }
}
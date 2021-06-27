using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tettris.Domain.Interface.Tetronimo
{
    public interface ITetromino
    {
        Guid TetronimoId { get; }
        IList<IBaseTetromino> BaseTetrominos { get; }
        event EventHandler<Vector3> OnMove;
        event EventHandler<IList<Vector2>> OnRotate;

        Vector3 StartPosition(List<Vector3> startPositions);
        void Move(Vector2 newPos);
        void Rotate(IList<Vector2> newPos);
    }
}
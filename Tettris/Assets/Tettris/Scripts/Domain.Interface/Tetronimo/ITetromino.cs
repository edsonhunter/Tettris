using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tettris.Domain.Interface.Tetronimo
{
    public interface ITetromino
    {
        Guid TetronimoId { get; }
        TetrominoType TetrominoType { get; }
        IList<IBaseTetromino> BaseTetrominos { get; }
        event EventHandler<Vector2> OnMove;
        event EventHandler<IList<Vector2>> OnRotate;

        Vector2 StartPosition(List<Vector2> startPositions);
        void Move(Vector2 newPos);
        void Rotate(IList<Vector2> newPos);
    }
}
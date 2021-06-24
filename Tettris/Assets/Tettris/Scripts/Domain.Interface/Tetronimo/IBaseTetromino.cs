using System;
using Tettris.Domain.Interface.Board;
using UnityEngine;

namespace Tettris.Domain.Interface.Tetronimo
{
    public interface IBaseTetromino
    {
        Guid TetronimoId { get; }
        Vector2 GridPosition { get; }
        int Color { get; }
        Vector2 Rotate(Vector2 rotateVector);
        Vector2 Move(Vector2 newPos); 
    }
}
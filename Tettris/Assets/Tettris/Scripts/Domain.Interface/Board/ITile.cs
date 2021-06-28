using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Interface.Board
{
    public interface ITile
    {
        Vector2 Position { get; }
        bool Occupy { get; }
        IBaseTetromino CurrentTetromino { get; }

        bool CheckSlot(IBaseTetromino tetromino);
        bool OccupySlot(IBaseTetromino tetromino);
        void ReleaseSlot();
    }
}
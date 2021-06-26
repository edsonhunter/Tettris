using System.Collections.Generic;
using Tettris.Domain.Interface.Tetronimo;

namespace Tettris.Domain.Interface.Board
{
    public interface IBoard
    {
        ITile[,] Tiles { get; }

        void StartNewTetromino(IList<IBaseTetromino> startTetrominos);
        bool Move(IList<IBaseTetromino> movedTetrominos);
        bool Rotate(IList<IBaseTetromino> moveTetrominos);
        bool CompleteLine();
        void ClearOldState(IList<IBaseTetromino> oldTetrominos);
    }
}
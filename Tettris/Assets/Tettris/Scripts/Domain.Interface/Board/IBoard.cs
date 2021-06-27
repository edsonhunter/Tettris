using System.Collections.Generic;
using Tettris.Domain.Interface.Tetronimo;

namespace Tettris.Domain.Interface.Board
{
    public interface IBoard
    {
        ITile[,] Tiles { get; }

        bool StartNewTetromino(IList<IBaseTetromino> startTetrominos);
        bool Move(IList<IBaseTetromino> movedTetrominos);
        bool Rotate(IList<IBaseTetromino> moveTetrominos);
        void FinishTurno(IList<IBaseTetromino> tetrominoBaseTetrominos);
        bool CompleteLine();
    }
}

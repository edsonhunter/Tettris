using System.Collections.Generic;
using Tettris.Domain.Interface.Tetronimo;

namespace Tettris.Domain.Interface.Board
{
    public interface IBoard
    {
        ITile[,] Tiles { get; }

        bool Move(IList<IBaseTetromino> movedTetrominos);
        bool Rotate(IList<IBaseTetromino> moveTetrominos);
        bool FinishTurno(IList<IBaseTetromino> tetrominoBaseTetrominos);
        IList<int> CompleteLine();
    }
}

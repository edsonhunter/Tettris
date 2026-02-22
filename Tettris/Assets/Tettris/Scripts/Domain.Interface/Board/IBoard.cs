using System.Collections.Generic;
using Tettris.Domain.Interface.Tetronimo;

namespace Tettris.Domain.Interface.Board
{
    public interface IBoard
    {
        ITile[,] Tiles { get; }
        int Colunas { get; }
        int Linhas { get; }
        bool Move(IList<IBaseTetromino> movedTetrominos);
        bool Rotate(IList<IBaseTetromino> moveTetrominos);
        bool FinishTurno(IList<IBaseTetromino> tetrominoBaseTetrominos);
        IList<int> CompleteLine();
    }
}

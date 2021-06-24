using System.Collections.Generic;
using Tettris.Domain.Interface.Tetronimo;

namespace Tettris.Domain.Interface.Board
{
    public interface IBoard
    {
        ITile[,] Tiles { get; }

        void StartNewTetromino(ITetromino newTetromino);
        bool Move(IList<IBaseTetromino> cubesNewPos);
        void ClearOldState(ITetromino tetromino);
        bool CompleteLine();
    }
}
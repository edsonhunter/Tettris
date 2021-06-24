using System;
using System.Collections.Generic;

namespace Tettris.Domain.Interface.Tetronimo
{
    public interface ITetromino
    {
        Guid TetronimoId { get; }
        IList<IBaseTetromino> BaseTetrominos { get; }

        IList<IBaseTetromino> Rotate();
        IList<IBaseTetromino> Move(int x, int y);
    }
}
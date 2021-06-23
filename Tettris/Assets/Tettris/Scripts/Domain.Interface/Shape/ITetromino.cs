using System.Collections.Generic;

namespace Tettris.Scripts.Domain.Interface
{
    public interface ITetromino
    {
        IList<IBaseTetromino> BaseTetrominos { get; }

        bool Move();
        bool Rotate();
    }
}
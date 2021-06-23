using Tettris.Scripts.Domain.Interface;

namespace Tettris.Domain.Shape
{
    public abstract class BaseTetromino : IBaseTetromino
    {
        public int[,] GridPosition { get; private set; }
        public int Color { get; private set; }
        
        private BaseTetromino(int[,] gridPosition, int color)
        {
            GridPosition = gridPosition;
            Color = color;
        }

        public virtual bool Move(int[,] newPos)
        {
            GridPosition = newPos;
            return true;
        }

        public virtual bool Rotate()
        {
            return true;
        }
    }
}
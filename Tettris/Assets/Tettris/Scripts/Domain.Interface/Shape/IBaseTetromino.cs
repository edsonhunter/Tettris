using UnityEngine;

namespace Tettris.Scripts.Domain.Interface
{
    public interface IBaseTetromino
    {
        int[,] GridPosition { get; }
        int Color { get; }

        bool Move(int[,] newPos); 
    }
}
using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Services.Interface
{
    public interface IGameService
    {
        IBoard Board { get; }
        ITetromino Tetromino { get; }
        float CurrentLevel { get; }
        bool Running { get; }

        void CreateNewBoard();
        ITetromino StartNewTetromino();
        bool NexTurno();
        void Move(Vector3 newPos);
        void Rotate(Quaternion newPos);
    }
}
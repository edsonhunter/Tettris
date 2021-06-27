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

        void CreateNewBoard(int linhas, int colunas);
        ITetromino StartNewTetromino();
        void Move(Vector3 newPos);
        void Rotate(Quaternion newPos);
        bool NextTurno();
        IList<int> CompleteLine();
        bool GameOver();
    }
}
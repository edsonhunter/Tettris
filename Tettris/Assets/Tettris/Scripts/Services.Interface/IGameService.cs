using System.Collections.Generic;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Services.Interface
{
    public interface IGameService : IService
    {
        IBoard Board { get; }
        ITetromino Tetromino { get; }
        float CurrentLevel { get; }
        bool Running { get; }

        void CreateNewBoard(int linhas, int colunas);
        ITetromino NextRound();
        void Move(Vector3 direction);
        void Rotate(Quaternion direction);
        bool IsFastDropping { get; set; }
        bool NextTurno();
        IList<int> CompleteLine();
        bool GameOver();
        float Speed();
    }
}
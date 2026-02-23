using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Services.Interface
{
    public interface IGameService : IService
    {
        float CurrentLevel { get; }
        bool IsFastDropping { get; set; }

        void CreateNewBoard(int linhas, int colunas);
        void Move(Vector3 direction);
        void Rotate(Quaternion direction);

        Task StartGameAsync(CancellationToken token);
        event Action<ITetromino> OnTetrominoSpawned;
        event Action OnPieceLanded;
        event Action<int> OnLinesCleared;
        event Action<float> OnLevelChanged;
        event Action OnGameOver;
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Tettris.Domain.Interface.Tetronimo;
using System.Numerics;

namespace Tettris.Services.Interface
{
    public interface IGameService : IService
    {
        float CurrentLevel { get; }
        bool IsFastDropping { get; set; }

        void CreateNewBoard(int linhas, int colunas);
        void Move(Vector2 direction);
        void Rotate(float angleDegrees);

        Task StartGameAsync(CancellationToken token);
        event Action<ITetromino> OnTetrominoSpawned;
        event Action OnPieceLanded;
        event Action<int> OnLinesCleared;
        event Action<float> OnLevelChanged;
        event Action OnGameOver;
    }
}
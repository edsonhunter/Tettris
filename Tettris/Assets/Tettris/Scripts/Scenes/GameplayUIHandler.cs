using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tettris.Scenes.Gameplay
{
    public class GameplayUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        
        [SerializeField]
        private TextMeshProUGUI _levelText;

        [SerializeField]
        private Button _pauseButton;

        public void Initialize(Action onPauseClick)
        {
            _pauseButton.onClick.AddListener(() => onPauseClick?.Invoke());
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = $"SCORE: {score}";
        }

        public void UpdateLevel(float level)
        {
            _levelText.text = $"LEVEL: {level}";
        }
    }
}

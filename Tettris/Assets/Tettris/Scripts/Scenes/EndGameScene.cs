using System;
using System.Collections;
using System.Collections.Generic;
using Tettris.Manager.Interface;
using Tettris.Scenes.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tettris.Scenes
{
    public class EndGameScene : BaseScene<EndGameData>
    {
        [SerializeField]
        private TextMeshProUGUI _score = null;
        public TextMeshProUGUI Score => _score;

        [SerializeField]
        private Button _closeButton = null;
        private Button CloseButton => _closeButton;
        
        protected override void Loading(Action<bool> loaded)
        {
            Score.text = SceneData.Score.ToString();
            CloseButton.onClick.AddListener(OnCloseClick);
        }

        private void OnCloseClick()
        {
            GetManager<ISceneManager>().UnloadOverlay(this);
        }
    }

    public class EndGameData : ISceneData
    {
        public int Score { get; private set; }

        public EndGameData(int score)
        {
            Score = score;
        }
    }
}


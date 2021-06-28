using System;
using Tettris.Manager.Interface;
using Tettris.Scenes.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Tettris.Scenes
{
    public class MainMenuScene : BaseScene<MainMenuData>
    {
        [SerializeField]
        private Button _playButton = null;
        public Button PlayButton => _playButton;

        protected override void Loading(Action<bool> loaded)
        {
            PlayButton.onClick.AddListener(OnPlayClick);
        }

        private void OnPlayClick()
        {
            GetManager<ISceneManager>().LoadScene(new GameplayScene.GamePlayData(20,10));
        }
    }

    public class MainMenuData : ISceneData
    {
        
    }
}


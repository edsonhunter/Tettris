using System;
using Tettris.Manager.Interface;
using Tettris.Scenes.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tettris.Scenes
{
    public class InGameMenuScene : BaseScene<InGameMenuData>
    {
        [SerializeField]
        private Button _resume = null;
        private Button Resume => _resume;

        [SerializeField]
        private Button _home = null;
        private Button Home  => _home;

        [SerializeField]
        private TextMeshProUGUI _pause = null;
        private TextMeshProUGUI Pause => _pause;
        
        protected override void Loading(Action<bool> loaded)
        {
            Pause.text = $"Pause";
            Resume.onClick.AddListener(OnResumeClick);
            Home.onClick.AddListener(OnHomeClick);
            
            loaded(true);
        }

        protected override void Loaded()
        {
            StartCoroutine(GetManager<IAudioManager>().FadeIn(0.1f));
            Time.timeScale = 0;
        }

        protected override void Unload()
        {
            StartCoroutine(GetManager<IAudioManager>().FadeIn(1f));
            Time.timeScale = 1;
        }

        private void OnResumeClick()
        {
            GetManager<ISceneManager>().UnloadOverlay(this);
        }

        private void OnHomeClick()
        {
            GetManager<ISceneManager>().UnloadOverlay(this);
            GetManager<ISceneManager>().LoadScene(new MainMenuData());
        }
    }

    public class InGameMenuData : ISceneData
    {
    
    }    
}


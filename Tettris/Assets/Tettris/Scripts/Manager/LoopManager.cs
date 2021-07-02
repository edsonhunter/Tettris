using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scenes.Interface;
using UnityEngine;

namespace Tettris.Manager
{
    public class LoopManager : MonoBehaviour, ILoopManager
    {
        private IBaseScene BaseScene { get; set; }
        private bool Registered { get; set; }
        
        public void RegisterNewScene(IBaseScene activeScene)
        {
            BaseScene = activeScene;
            Registered = true;
        }

        public void UnregisterScene()
        {
            Registered = false;
            BaseScene = null;
        }

        public void Update()
        {
            Loop();
        }

        private void Loop()
        {
            if (Registered)
            {
                if(BaseScene.ActiveScene)
                    BaseScene.FireLoop();
            }
        }
    }
}
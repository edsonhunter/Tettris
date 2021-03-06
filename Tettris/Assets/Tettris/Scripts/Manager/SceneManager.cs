using System;
using System.Collections;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using Tettris.Scenes.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tettris.Manager
{
    public class SceneManager : MonoBehaviour, ISceneManager
    {
        private BaseScene ActiveScene = null;
        private IApplication Application { get; set; }
        
        public void Init(IApplication application)
        {
            Application = application;
        }

        private void SetupSceneToLoad()
        {
            if (ActiveScene != null)
            {
                ActiveScene.SetActiveScene(false);
                ActiveScene.FireUnload();
                Application.GetManager<ILoopManager>().UnregisterScene();
            }
        }

        public void LoadScene(ISceneData data)
        {
            SetupSceneToLoad();
            
            UnitySceneManager.LoadSceneAsync(data.GetType().Name.Replace("Data", "Scene"), LoadSceneMode.Single).completed += operation =>
            {
                Resources.UnloadUnusedAssets();
                
                ActiveScene = GetActiveSceneController();
                ActiveScene.Init(Application, data);
                ActiveScene.SetActiveScene(true);
                ActiveScene.FireLoading(loaded =>
                {
                   ActiveScene.FireLoaded();
                   Application.GetManager<ILoopManager>().RegisterNewScene(ActiveScene);
                });
            };
        }

        public void LoadSceneOverlay(ISceneData data)
        {
            UnitySceneManager.LoadSceneAsync(data.GetType().Name.Replace("Data", "Scene"), LoadSceneMode.Additive).completed += operation =>
            {
                SetLastLoadedSceneActive();
                var overlay = GetActiveSceneController();
                overlay.Init(Application, data);
                ActiveScene.SetActiveScene(false);
                overlay.FireLoading(loaded =>
                {
                    overlay.FireLoaded(); 
                });
            };
        }
        
        public void UnloadOverlay(IBaseScene overlay)
        {
            overlay.FireUnload();
            UnitySceneManager.UnloadSceneAsync(UnitySceneManager.GetActiveScene()).completed += (operation) =>
            {
                ActiveScene.SetActiveScene(true);
            };
        }

        private BaseScene GetActiveSceneController()
        {
            Scene activeScene = UnitySceneManager.GetActiveScene();
            GameObject[] overlayRootObjects = activeScene.GetRootGameObjects();

            BaseScene baseScene = null;
            foreach (GameObject rootObject in overlayRootObjects)
            {
                if (rootObject.GetComponent<BaseScene>() == null)
                    continue;
                
                baseScene = rootObject.GetComponent<BaseScene>();
            }

            return baseScene;
        }
        
        private void SetLastLoadedSceneActive()
        {
            Scene lastLoadedScene = default;
            var lastSceneIndex = UnitySceneManager.sceneCount - 1;

            // Quando pedimos para a Unity carregar uma cena assincronamente ela conta na lista de cenas,
            // por??m ela n??o pode ser colocada como ativa at?? terminar de ser carregada totalmente
            while (lastSceneIndex >= 0)
            {
                lastLoadedScene = UnitySceneManager.GetSceneAt(lastSceneIndex);

                if (lastLoadedScene.IsValid() && lastLoadedScene.isLoaded)
                    break;

                lastSceneIndex--;
            }

            UnitySceneManager.SetActiveScene(lastLoadedScene);
        }
    }
}
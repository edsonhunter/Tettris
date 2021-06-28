using System;
using Tettris.Manager.Interface;
using Tettris.Scenes.Interface;
using Tettris.Services.Interface;
using UnityEngine;

namespace Tettris.Scenes
{
    public abstract class BaseScene : MonoBehaviour, IBaseScene
    {
        internal ISceneData SceneData { get; private set; }
        internal IApplication Application { get; private set; }
        public bool ActiveScene { get; private set; }
        
        public void Init(IApplication application, ISceneData sceneData)
        {
            Application = application;
            SceneData = sceneData;
        }
        
        protected virtual void Loading(Action<bool> loaded) { }
        protected virtual void Loaded() { }
        protected virtual void Loop() { }
        protected virtual void Unload() { }

        public void FireLoading(Action<bool> loaded)
        {
            Loading(loaded);
        }

        public void FireLoaded()
        {
            Loaded();
        }

        public void FireLoop()
        {
            Loop();
        }

        public void FireUnload()
        {
            Unload();
        }

        protected T GetService<T>() where T : IService
        {
            return Application.GetService<T>();
        }

        protected T GetManager<T>() where T : IManager
        {
            return Application.GetManager<T>();
        }

        public void SetActiveScene(bool isActive)
        {
            ActiveScene = isActive;
        }
    }
    
    public abstract class BaseScene<T> : BaseScene where T : class, ISceneData
    {
        public new T SceneData => (T) base.SceneData;
    }
}
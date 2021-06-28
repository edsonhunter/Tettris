using System;
using System.Collections.Generic;
using Tettris.Manager.Interface;
using Tettris.Scenes;
using Tettris.Scripts.Services;
using Tettris.Services.Interface;
using UnityEngine;

namespace Tettris.Manager
{
    public class ApplicationManager : MonoBehaviour, IApplication
    {
        #region Managers
        [SerializeField]
        private SceneManager _sceneManager = null;
        [SerializeField]
        private LoopManager _loopManager = null;
        [SerializeField]
        private AudioManager _audioManager = null;

        private IDictionary<Type, IManager> Managers { get; set; }
        
        #endregion
        
        #region Services
        private IDictionary<Type, IService> Services { get; set; }
        private IGameService GameService { get; set; }
        private ISettingsService SettingsService { get; set; }
        #endregion
        
        private void Awake()
        {
            Services = new Dictionary<Type, IService>();
            Managers = new Dictionary<Type, IManager>();
            BootServices();
            BootManager();
            _sceneManager.LoadScene(new MainMenuData());
        }

        private void BootServices()
        {
            GameService = new GameService();
            RegisterService<IGameService>(GameService);
            SettingsService = new SettingsService();
            RegisterService<ISettingsService>(SettingsService);
        }
        
        private void BootManager()
        {
            _sceneManager.Init(this);
            _audioManager.Init(this);
            RegisterManager<ISceneManager>(_sceneManager);
            RegisterManager<ILoopManager>(_loopManager);
            RegisterManager<IAudioManager>(_audioManager);
        }
        
        private void RegisterService<T>(IService service) where T : IService
        {
            if (!(service is T))
            {
                throw new InvalidOperationException(
                    $"Nao pode registar o Manager {service.GetType().FullName} para a interface {typeof(T).FullName}");
            }

            Services.Add(typeof(T), service);
        }

        private void RegisterManager<T>(IManager manager) where T : IManager
        {
            if (!(manager is T))
            {
                throw new InvalidOperationException(
                    $"Nao pode registar o Manager {manager.GetType().FullName} para a interface {typeof(T).FullName}");
            }
            
            Managers.Add(typeof(T), manager);
        }

        public T GetService<T>() where T : IService
        {
            try
            {
                return (T) Services[typeof(T)];
            }
            catch (KeyNotFoundException e)
            {
                if (typeof(T).IsInterface)
                {
                    throw new KeyNotFoundException($"O serviço '{typeof(T).FullName}' não foi registrado.", e);
                }

                throw new KeyNotFoundException($"O serviço '{typeof(T).FullName}' não é uma interface.", e);
            }
        }

        public T GetManager<T>() where T : IManager
        {
            return (T) Managers[typeof(T)];
        }
    }
}
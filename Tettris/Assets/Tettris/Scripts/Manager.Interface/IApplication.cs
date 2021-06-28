using Tettris.Services.Interface;

namespace Tettris.Manager.Interface
{
    public interface IApplication : IManager
    {
        public T GetService<T>() where T : IService;
        public T GetManager<T>() where T : IManager;
    }
}


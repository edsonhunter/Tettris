using Tettris.Scenes.Interface;

namespace Tettris.Manager.Interface
{
    public interface ILoopManager : IManager
    {
        void RegisterNewScene(IBaseScene activeScene);
        void UnregisterScene();
    }
}
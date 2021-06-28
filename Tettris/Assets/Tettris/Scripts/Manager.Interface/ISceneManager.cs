using Tettris.Scenes;
using Tettris.Scenes.Interface;

namespace Tettris.Manager.Interface
{
    public interface ISceneManager : IManager
    {
        void LoadScene(ISceneData gamePlayData);
        public void LoadSceneOverlay(ISceneData data);
        void UnloadOverlay(IBaseScene overlay);
    }
}
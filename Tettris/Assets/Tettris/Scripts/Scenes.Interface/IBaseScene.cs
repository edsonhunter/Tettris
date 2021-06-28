using System;

namespace Tettris.Scenes.Interface
{
    public interface IBaseScene
    {
        public void FireLoading(Action<bool> loaded);
        public void FireLoaded();
        public void FireLoop();
        public void FireUnload();
        
        bool ActiveScene { get; }
    }
}
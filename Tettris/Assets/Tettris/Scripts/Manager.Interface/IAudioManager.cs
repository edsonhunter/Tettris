using System.Collections;

namespace Tettris.Manager.Interface
{
    public interface IAudioManager : IManager
    {
        void Play();
        void Pause();
        IEnumerator FadeIn(float target);
        IEnumerator FadeOut(float target);
    }
}
using System.Collections;
using Tettris.Manager.Interface;
using Tettris.Services.Interface;
using UnityEngine;

namespace Tettris.Manager
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        private IApplication Application { get; set; }
        private ISettingsService SettingsService { get; set; }
        
        [SerializeField]
        private AudioSource _source = null;
        protected AudioSource Source => _source;

        public void Init(IApplication application)
        {
            Application = application;
            SettingsService = Application.GetService<ISettingsService>();
        }
        
        public void Play()
        {
            if (SettingsService.MusicOn && !_source.isPlaying)
            {
                _source.Play();   
            }
        }

        public void Pause()
        {
            if (SettingsService.MusicOn && _source.isPlaying)
            {
                _source.Pause();
            }
        }

        public IEnumerator FadeIn(float target)
        {
            Play();
            yield return FadeTo(target);
        }
        
        public IEnumerator FadeOut(float target)
        {
            yield return FadeTo(target);
        }
        
        private IEnumerator FadeTo(float targetVolume, float timeToFade = 1)
        {
            var volumeDelta = Mathf.Clamp01(targetVolume) - Source.volume;
            while (Source.isPlaying && Source.volume != targetVolume)
            {
                var newVolume = Source.volume + volumeDelta * (timeToFade <= 0 ? 1 : Time.deltaTime / timeToFade);
                Source.volume = volumeDelta < 0 ? Mathf.Max(newVolume, targetVolume) : Mathf.Min(newVolume, targetVolume);
                yield return null;
            }
        }
    }
}
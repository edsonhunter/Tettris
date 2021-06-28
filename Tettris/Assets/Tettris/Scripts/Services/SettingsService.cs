using Tettris.Services.Interface;

namespace Tettris.Scripts.Services
{
    public class SettingsService : ISettingsService
    {
        public bool MusicOn { get; private set; }

        public SettingsService()
        {
            MusicOn = true;
        }
        
        public void ToggleMusic()
        {
            MusicOn = !MusicOn;
        }
    }
}
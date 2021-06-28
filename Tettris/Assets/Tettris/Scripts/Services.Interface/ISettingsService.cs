namespace Tettris.Services.Interface
{
    public interface ISettingsService : IService
    {
        bool MusicOn { get; }
        void ToggleMusic();
    }
}
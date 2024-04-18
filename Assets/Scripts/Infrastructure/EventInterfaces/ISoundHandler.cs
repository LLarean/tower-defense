namespace Infrastructure
{
    public interface ISoundHandler : IGlobalSubscriber
    {
        void HandleClick();
        void HandleCast();
        void HandleMusicVolume(float value);
        void HandleSoundVolume(float value);
        void HandleLoadMenu();
        void HandleLoadGame();
    }
}

namespace Infrastructure
{
    public interface ISoundHandler : IGlobalSubscriber
    {
        void HandleClick();
        void HandleConstruction();
        void HandleCast();
        void HandleMusicVolume(float value);
        void HandleSoundVolume(float value);
        void HandleLoadMenu();
        void HandleLoadGame();
    }
}

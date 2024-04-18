namespace Infrastructure
{
    public interface ISoundHandler : IGlobalSubscriber
    {
        void HandleClick();
        void HandleLoadMenu();
        void HandleLoadGame();
    }
}

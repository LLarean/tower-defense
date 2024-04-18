namespace Infrastructure
{
    public interface ISoundHandler : IGlobalSubscriber
    {
        void HandleClick();
        void HandleCast();
        void HandleLoadMenu();
        void HandleLoadGame();
    }
}

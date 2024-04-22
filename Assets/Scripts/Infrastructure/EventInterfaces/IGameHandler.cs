namespace Infrastructure
{
    public interface IGameHandler : IGlobalSubscriber
    {
        void HandleFinishMatch();
    }
}
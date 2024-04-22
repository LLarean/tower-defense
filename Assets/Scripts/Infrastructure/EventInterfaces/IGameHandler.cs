namespace Infrastructure
{
    public interface IGameHandler : IGlobalSubscriber
    {
        void HandleStartRound();
        void HandleFinishRound();
        void HandleFinishMatch();
    }
}
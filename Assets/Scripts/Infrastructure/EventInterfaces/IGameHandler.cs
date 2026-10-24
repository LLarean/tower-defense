namespace Infrastructure
{
    public interface IGameHandler : IGlobalSubscriber
    {
        void HandlePrepareRound();
        void HandleStartRound();
        void HandleStopRound();
    }
}
namespace Infrastructure
{
    public interface IEnemyHandler : IGlobalSubscriber
    {
        void HandleDestroy();
        void HandleFinishRoute();
    }
}

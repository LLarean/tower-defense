namespace Infrastructure
{
    public interface IEnemyHandler : IGlobalSubscriber
    {
        void HandleEnemyDestroy();
        void HandleFinishRoute();
    }
}

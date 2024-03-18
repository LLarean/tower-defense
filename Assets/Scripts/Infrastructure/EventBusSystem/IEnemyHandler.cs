using EventBusSystem;

public interface IEnemyHandler : IGlobalSubscriber
{
    void HandleDestroy();
    void HandleFinish();
}

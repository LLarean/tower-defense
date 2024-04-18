namespace Infrastructure
{
    public interface IUpdateHandler : IGlobalSubscriber
    {
        void HandleUpdate();
    }
}
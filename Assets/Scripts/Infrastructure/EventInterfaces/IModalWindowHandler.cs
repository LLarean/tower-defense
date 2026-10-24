namespace Infrastructure
{
    public interface IModalWindowHandler : IGlobalSubscriber
    {
        void HandleShow();
        void HandleHide();
    }
}
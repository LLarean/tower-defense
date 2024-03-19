namespace Infrastructure
{
    public interface IInputHandler : IGlobalSubscriber
    {
        void HandleMousePosition(int positionX, int positionY);
        void HandleBuild();
        void HandleCancel();
        void HandleMenu();
    }
}
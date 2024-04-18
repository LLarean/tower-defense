using UnityEngine;

namespace Infrastructure
{
    public interface IInputHandler : IGlobalSubscriber
    {
        void HandleMousePosition(Vector2 mousePosition);
        void HandleBuild();
        void HandleCancel();
        void HandleMenu();
    }
}
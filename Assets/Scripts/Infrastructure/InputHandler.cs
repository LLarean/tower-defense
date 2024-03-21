using UnityEngine;

namespace Infrastructure
{
    public class InputHandler : MonoBehaviour
    {
        private int _mousePositionX;
        private int _mousePositionY;

        // TODO add settings
        private int _buildButton = 0;
        private int _cancelButton = 1;
        private KeyCode _menuButton = KeyCode.Escape;
    
        private void Update()
        {
            // EventBus.RaiseEvent<IUpdateHandler>(handler => handler.HandleUpdate());
            ChangeMousePosition();
        
            if (Input.GetMouseButtonDown(_buildButton) == true)
            {
                EventBus.RaiseEvent<IInputHandler>(handler => handler.HandleBuild());
            }

            if (Input.GetMouseButtonDown(_cancelButton) == true)
            {
                EventBus.RaiseEvent<IInputHandler>(handler => handler.HandleCancel());
            }

            if (Input.GetMouseButtonDown(2) == true)
            {
            }
        
            if (Input.GetKeyDown(_menuButton))
            {
                EventBus.RaiseEvent<IInputHandler>(handler => handler.HandleMenu());
            }
        }

        private void ChangeMousePosition()
        {
            var positionX = (int)Input.mousePosition.x;
            var positionY = (int)Input.mousePosition.y;
        
            if (_mousePositionX != positionX || positionY != (int)Input.mousePosition.y)
            {
                _mousePositionX = positionX;
                _mousePositionY = positionY;
            
                EventBus.RaiseEvent<IInputHandler>(handler => handler.HandleMousePosition(_mousePositionX, _mousePositionY));
            }
        }
    }
}
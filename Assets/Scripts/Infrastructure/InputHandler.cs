using System;
using UnityEngine;

namespace Infrastructure
{
    public class InputHandler : MonoBehaviour
    {
        private Vector2 _mousePosition;
        
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
            var mousePosition = new Vector2
            {
                x = Input.mousePosition.x,
                y = Input.mousePosition.y
            };
            
            var tolerance = 1;

            if (Math.Abs(_mousePosition.x - mousePosition.x) > tolerance ||
                Math.Abs(_mousePosition.y - Input.mousePosition.y) > tolerance)
            {
                _mousePosition = mousePosition;

                EventBus.RaiseEvent<IInputHandler>(handler =>
                    handler.HandleMousePosition(_mousePosition));
            }
        }
    }
}
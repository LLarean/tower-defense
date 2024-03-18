using System;
using UnityEngine;

public class InputHandler : MonoBehaviour//, IInputHandler
{
    private int _mousePositionX;
    private int _mousePositionY;
    
    private int _buildButton = 0;
    private int _cancelButton = 1;
    private KeyCode _menuButton = KeyCode.Escape;
    
    
    // TODO Convert actions to an event bus call
    public event Action<int, int> OnMousePositionChanged;
    
    public event Action OnBuildClicked;
    public event Action OnCancelClicked;
    public event Action OnMenuClicked;
    
    private void Update()
    {
        ChangeMousePosition();
        
        if (Input.GetMouseButtonDown(_buildButton) == true)
        {
            OnBuildClicked?.Invoke();
        }

        if (Input.GetMouseButtonDown(_cancelButton) == true)
        {
            OnCancelClicked?.Invoke();
        }

        if (Input.GetMouseButtonDown(2) == true)
        {
        }
        
        if (Input.GetKeyDown(_menuButton))
        {
            OnMenuClicked?.Invoke();
        }
    }

    private void ChangeMousePosition()
    {
        var mousePositionX = (int)Input.mousePosition.x;
        var mousePositionY = (int)Input.mousePosition.y;
        
        if (_mousePositionX != mousePositionX || mousePositionY != (int)Input.mousePosition.y)
        {
            _mousePositionX = mousePositionX;
            _mousePositionY = mousePositionY;
            
            OnMousePositionChanged?.Invoke(_mousePositionX, _mousePositionY);
        }
    }
}
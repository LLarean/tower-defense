using System;
using UnityEngine;

public class InputHandler : MonoBehaviour//, IInputHandler
{
    private int _buildButton = 0;
    private int _cancelButton = 1;
    private KeyCode _menuButton = KeyCode.Escape;
    
    public event Action OnBuildClicked;
    public event Action OnCancelClicked;
    public event Action OnMenuClicked;
    
    private void Update()
    {
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
}
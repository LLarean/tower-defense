using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Vector3 _position;
    
    private float _speedMovement = 40f;
    private float _borderThickness = 10f;
    
    private float _minimumZoom = 40f;
    private float _maximumZoom = 70f;

    private void Start()
    {
        _position = transform.position;
    }

    private void Update()
    {
        ScrollCamera();
        ZoomCamera();
    }

    private void ScrollCamera()
    {
        if (Input.mousePosition.x < _borderThickness)
        {
            _position.x -= _speedMovement * Time.deltaTime;
        }
        else if (Input.mousePosition.x > Screen.width - _borderThickness)
        {
            _position.x += _speedMovement * Time.deltaTime;
        }

        if (Input.mousePosition.y < _borderThickness)
        {
            _position.z -= _speedMovement * Time.deltaTime;
        }
        else if (Input.mousePosition.y > Screen.height - _borderThickness)
        {
            _position.z += _speedMovement * Time.deltaTime;
        }
        
        transform.position = _position;
    }

    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            _position.y -= Input.mouseScrollDelta.y;
        }

        if (_position.y < _minimumZoom)
        {
            _position.y = _minimumZoom;
        }
        else if (_position.y > _maximumZoom)
        {
            _position.y = _maximumZoom;
        }

        transform.position = _position;
    }
}
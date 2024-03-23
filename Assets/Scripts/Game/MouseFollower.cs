using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private Collider _terrainCollider;
    private float _heightOffset = 0f; 
        
    private int _movingStep = 20;
    private Camera _mainCamera;

    private bool _сanBuild;
    public bool CanBuild => _сanBuild;

    public void Initialize(Collider terrainCollider)
    {
        _terrainCollider = terrainCollider;
    }

    public void MousePositionChange(float positionX, float positionY)
    {
        if (_terrainCollider == null || _mainCamera == null)
        {
            return;
        }
        
        positionX = Mathf.Round(positionX / _movingStep) * _movingStep;
        positionY = Mathf.Round(positionY / _movingStep) * _movingStep;

        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(positionX, positionY, 0));
        RaycastHit hit;

        if (_terrainCollider.Raycast(ray, out hit, Mathf.Infinity) == true)
        {
            Vector3 newPosition = hit.point + new Vector3(0, _heightOffset, 0);
            transform.position = newPosition;
            _сanBuild = true;
        }
        else
        {
            _сanBuild = false;
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }
}
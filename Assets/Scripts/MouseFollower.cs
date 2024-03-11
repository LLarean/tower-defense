using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private Builder _builder;
    private Collider _terrainCollider;
    private float _heightOffset = 0f; 
        
    private int _movingStep = 20;

    public void Init(Builder builder, Collider terrainCollider)
    {
        _builder = builder;
        _terrainCollider = terrainCollider;
    }
        
    private void Update()
    {
        float positionX = (int)Input.mousePosition.x;
        float positionY = (int)Input.mousePosition.y;
            
        positionX = Mathf.Round(positionX / _movingStep) * _movingStep;
        positionY = Mathf.Round(positionY / _movingStep) * _movingStep;
            

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(positionX, positionY, 0));
        RaycastHit hit;

        if (_terrainCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 newPosition = hit.point + new Vector3(0, _heightOffset, 0);
            transform.position = newPosition;
        }

        // TODO it is necessary to separate it into a separate entity to control the click
        if (Input.GetMouseButtonDown(0))
        {
            _builder.ConstructBuilding();
        }
    }
}
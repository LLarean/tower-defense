using System.Collections.Generic;
using Builds;
using NaughtyAttributes;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings;
    [SerializeField] private Collider _terrainCollider;

    private Building _currentBuilding;
    
    private void Update()
    {
        // TODO it is necessary to separate it into a separate entity to control the click
        if (Input.GetKeyDown(KeyCode.Escape) == true && _currentBuilding != null)
        {
            DisableConstructionMode();
        }
    }

    [Button()]
    private void BuildTower()
    {
        EnableConstructionMode();
        InstantiateBuild(_buildings[0]);
    }
    
    private void InstantiateBuild(Building building)
    {
        EnableConstructionMode();
        ClearFollowingBuilding();
                
        _currentBuilding = Instantiate(building, transform.position, Quaternion.identity);
        _currentBuilding.Initialize(this, _terrainCollider);
    }
    
    private void EnableConstructionMode()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    
    private void DisableConstructionMode()
    {
        Destroy(_currentBuilding.gameObject);
        _currentBuilding = null;
    }
    
    private void ClearFollowingBuilding()
    {
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding.gameObject);
            _currentBuilding = null;
        }
    }

    public void ConstructBuilding()
    {
        var temp = _currentBuilding;
        _currentBuilding = null;
            
        InstantiateBuild(temp);
    }
}
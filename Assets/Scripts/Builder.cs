using System.Collections.Generic;
using Builds;
using NaughtyAttributes;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings;
    [SerializeField] private Collider _terrainCollider;
    [SerializeField] private ConstructedBuildings _constructedBuildings;

    private Building _currentBuilding;

    public void ConstructBuilding()
    {
        _currentBuilding.DisableMouseFollower();
        _constructedBuildings.SetNewBuilding(_currentBuilding);
        var temp = _currentBuilding;
        _currentBuilding = null;
            
        InstantiateBuild(temp);
    }

    [Button()]
    public void BuildGunTower()
    {
        EnableConstructionMode();
        InstantiateBuild(_buildings[0]);
    }

    [Button()]
    public void BuildArrowTower()
    {
        EnableConstructionMode();
        InstantiateBuild(_buildings[1]);
    }

    private void Update()
    {
        // TODO it is necessary to separate it into a separate entity to control the click
        if (Input.GetKeyDown(KeyCode.Escape) == true && _currentBuilding != null)
        {
            DisableConstructionMode();
        }
        
        if (Input.GetMouseButtonDown(1) == true && _currentBuilding != null)
        {
            DisableConstructionMode();
        }
    }

    private void GetBuildings()
    {
        
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
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
#endif
    }

    [Button()]
    private void DisableConstructionMode()
    {
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding.gameObject);
            _currentBuilding = null;
        }
    }

    private void ClearFollowingBuilding()
    {
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding.gameObject);
            _currentBuilding = null;
        }
    }
}
using System.Collections.Generic;
using Builds;
using UnityEngine;
using Zenject;

public class Builder : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings;
    [SerializeField] private Collider _terrainCollider;
    [SerializeField] private ConstructedBuildings _constructedBuildings;

    private Building _currentBuilding;
    private HUD _hud;

    [Inject]
    public void Construct(HUD hud)
    {
        _hud = hud;
    }
    
    public void ConstructBuilding()
    {
        _currentBuilding.DisableMouseFollower();
        _constructedBuildings.SetNewBuilding(_currentBuilding);
        var temp = _currentBuilding;
        _currentBuilding = null;

        _hud.RemoveGold();
        
        InstantiateBuild(temp);
    }

    public void BuildFireTower()
    {
        EnableConstructionMode();
        InstantiateBuild(_buildings[0]);
    }

    public void BuildWaterTower()
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

    private void DisableConstructionMode()
    {
#if !UNITY_EDITOR
        Cursor.visible = true;
#endif
        
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding.gameObject);
            _currentBuilding = null;
        }
        
        _hud.ClearInfo();
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
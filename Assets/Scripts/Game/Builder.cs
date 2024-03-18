using System.Collections.Generic;
using Builds;
using UI.Game;
using UnityEngine;
using Zenject;

public class Builder : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings;
    [SerializeField] private Collider _terrainCollider;
    [SerializeField] private ConstructedBuildings _constructedBuildings;

    private Building _currentBuilding;
    private HUD _hud;
    private PlayerModel _playerModel;
    private InputHandler _inputHandler;

    [Inject]
    public void Construct(HUD hud, PlayerModel playerModel, InputHandler inputHandler)
    {
        _hud = hud;
        _playerModel = playerModel;
        _inputHandler = inputHandler;

        _playerModel.CurrentBuilding.ValueChanged += BuildTower;
        
        _inputHandler.OnMousePositionChanged += MousePositionChange;
        
        _inputHandler.OnBuildClicked += ConstructBuilding;
        _inputHandler.OnCancelClicked += DisableConstructionMode;
        _inputHandler.OnMenuClicked += DisableConstructionMode;
    }

    public void ConstructBuilding()
    {
        if (_currentBuilding == null)
        {
            return;
        }

        if (_playerModel.Gold.Value >= _playerModel.CurrentBuilding.Value.Price)
        {
            _currentBuilding.DisableMouseFollower();
            _constructedBuildings.SetNewBuilding(_currentBuilding);
            var temp = _currentBuilding;
            _currentBuilding = null;

            _playerModel.Gold.Value -= _playerModel.CurrentBuilding.Value.Price;
            InstantiateBuild(temp);
        }
    }

    // public void BuildFireTower()
    // {
    //     EnableConstructionMode();
    //     InstantiateBuild(_buildings[0]);
    // }

    // public void BuildWaterTower()
    // {
    //     EnableConstructionMode();
    //     InstantiateBuild(_buildings[1]);
    // }
    
    private void BuildTower(BuildModel current, BuildModel previous)
    {
        EnableConstructionMode();
        
        if (current.CastType == CastType.Fire)
        {
            InstantiateBuild(_buildings[0]);
        }
        else if(current.CastType == CastType.Ice)
        {
            InstantiateBuild(_buildings[1]);
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
    
    private void MousePositionChange(int positionX, int positionY)
    {
        if (_currentBuilding != null)
        {
            _currentBuilding.MousePositionChange(positionX, positionY);
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
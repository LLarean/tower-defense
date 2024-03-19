using System.Collections.Generic;
using Builds;
using Infrastructure;
using UI.Game;
using UnityEngine;
using Zenject;

public class Builder : MonoBehaviour, IInputHandler
{
    [SerializeField] private List<Building> _buildings;
    [SerializeField] private Collider _terrainCollider;
    [SerializeField] private ConstructedBuildings _constructedBuildings;

    private Building _currentBuilding;
    private HUD _hud;
    private PlayerModel _playerModel;

    [Inject]
    public void Construct(HUD hud, PlayerModel playerModel)
    {
        _hud = hud;
        _playerModel = playerModel;

        _playerModel.CurrentBuilding.ValueChanged += BuildTower;
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
    
    public void HandleMousePosition(int positionX, int positionY)
    {
        MousePositionChange(positionX, positionY);
    }

    public void HandleBuild()
    {
        ConstructBuilding();
    }

    public void HandleCancel()
    {
        DisableConstructionMode();
    }

    public void HandleMenu()
    {
        DisableConstructionMode();
    }

    private void Start()
    {
        EventBus.Subscribe(this);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(this);
    }

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
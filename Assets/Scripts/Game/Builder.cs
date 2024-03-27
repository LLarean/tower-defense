using System.Collections.Generic;
using Builds;
using Infrastructure;
using UI.Game;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Builder : MonoBehaviour, IInputHandler
    {
        [SerializeField] private List<Building> _buildings;
        [SerializeField] private Collider _terrainCollider;

        private ConstructedBuildings _constructedBuildings;
        private Tower _currentTower;
        private HUD _hud;
        private PlayerModel _playerModel;
        private TowerParams _towerParams;

        [Inject]
        public void Construct(HUD hud, PlayerModel playerModel, TowerParams towerParams)
        {
            _hud = hud;
            _playerModel = playerModel;
            _towerParams = towerParams;
            _constructedBuildings = new ConstructedBuildings();

            _playerModel.CurrentBuilding.ValueChanged += BuildTower;
        }

        public void ConstructBuilding()
        {
            if (_currentTower == null)
            {
                return;
            }

            // TODO the need to add a check if the mouse is outside the desired limits
            if (_currentTower.CanBuild == false)
            {
                return;
            }

            if (_playerModel.Gold.Value >= _playerModel.CurrentBuilding.Value.Price)
            {
                _currentTower.DisableConstructionMode();
                _constructedBuildings.SetNewBuilding(_currentTower);
                
                var temp = _currentTower;
                _currentTower = null;
                _playerModel.Gold.Value -= _playerModel.CurrentBuilding.Value.Price;
                
                InstantiateTower(temp.TowerModel);
            }
        }

        public void HandleMousePosition(Vector2 mousePosition)
        {
            MousePositionChange(mousePosition);
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
            ClearFollowingBuilding();

            var isSuccess = _towerParams.TryGetTowerModel(current.ElementalType, out TowerModel towerModel);

            if (isSuccess == true)
            {
                InstantiateTower(towerModel);
            }
            else
            {
                Debug.LogError("Class: 'Builder', Method: 'BuildTower', Message: 'isSuccess != true'");
            }
        }

        private void InstantiateTower(TowerModel towerModel)
        {
            _currentTower = Instantiate(towerModel.Tower, _terrainCollider.transform.position, Quaternion.identity);
            _currentTower.Initialize(_terrainCollider, towerModel);
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

            if (_currentTower != null)
            {
                Destroy(_currentTower.gameObject);
                _currentTower = null;
            }

            _hud.ClearInfo();
        }

        private void MousePositionChange(Vector2 mousePosition)
        {
            if (_currentTower != null)
            {
                _currentTower.MousePositionChange(mousePosition);
            }
        }

        private void ClearFollowingBuilding()
        {
            if (_currentTower != null)
            {
                Destroy(_currentTower.gameObject);
                _currentTower = null;
            }
        }
    }
}
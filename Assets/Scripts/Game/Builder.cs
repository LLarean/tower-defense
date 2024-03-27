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
        private Building _currentBuilding;
        private HUD _hud;
        private PlayerModel _playerModel;

        [Inject]
        public void Construct(HUD hud, PlayerModel playerModel)
        {
            _hud = hud;
            _playerModel = playerModel;
            _constructedBuildings = new ConstructedBuildings();

            _playerModel.CurrentBuilding.ValueChanged += BuildTower;
        }

        public void ConstructBuilding()
        {
            if (_currentBuilding == null)
            {
                return;
            }

            // TODO the need to add a check if the mouse is outside the desired limits
            if (_currentBuilding.CanBuild == false)
            {
                return;
            }

            if (_playerModel.Gold.Value >= _playerModel.CurrentBuilding.Value.Price)
            {
                _currentBuilding.DisableConstructionMode();
                // _constructedBuildings.SetNewBuilding(_currentBuilding);
                var temp = _currentBuilding;
                _currentBuilding = null;

                _playerModel.Gold.Value -= _playerModel.CurrentBuilding.Value.Price;
                InstantiateBuild(temp);
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

            var isSuccess = TryGetBuilding(current.ElementalType, out Building building);

            if (isSuccess == true)
            {
                InstantiateBuild(building);
            }
            else
            {
                Debug.LogError("Class: 'Builder', Method: 'BuildTower', Message: 'isSuccess != true'");
            }
        }

        private void InstantiateBuild(Building building)
        {
            _currentBuilding = Instantiate(building, _terrainCollider.transform.position, Quaternion.identity);
            _currentBuilding.Initialize(_terrainCollider);
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

        private void MousePositionChange(Vector2 mousePosition)
        {
            if (_currentBuilding != null)
            {
                _currentBuilding.MousePositionChange(mousePosition);
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

        private bool TryGetBuilding(ElementalType elementalType, out Building newBuilding)
        {
            bool isSuccess = false;
            newBuilding = null;

            foreach (var building in _buildings)
            {
                if (building is not Tower tower)
                {
                    continue;
                }
            
                if (tower.ElementalType == elementalType)
                {
                    newBuilding = building;
                    isSuccess = true;
                }
            }
        
            return isSuccess;
        }
    }
}
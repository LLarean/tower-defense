using Builds;
using GameUtilities;
using Infrastructure;
using UI.Game;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Builder : MonoBehaviour, IInputHandler
    {
        [SerializeField] private Collider _terrainCollider;

        private ConstructedBuildings _constructedBuildings;
        private TowerContainer _towerContainer;
        private Tower _currentTower;
        
        private HUD _hud;
        private PlayerModel _playerModel;
        private TowerParams _towerParams;
        private MouseFollower _mouseFollower;
        
        private bool _canBuild;

        [Inject]
        public void Construct(HUD hud, PlayerModel playerModel, TowerParams towerParams)
        {
            _hud = hud;
            _playerModel = playerModel;
            _towerParams = towerParams;
            _constructedBuildings = new ConstructedBuildings();

            _playerModel.CurrentBuilding.ValueChanged += BuildTower;
            
            if (_terrainCollider == null)
            {
                Debug.LogError("Class: 'Building', Method: 'Initialize', Message: 'terrainCollider == null'");
                return;
            }
            
            _mouseFollower = new MouseFollower(_terrainCollider);
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
                _playerModel.Gold.Value -= _playerModel.CurrentBuilding.Value.Price;
                _currentTower = null;
                
                InstantiateTower(_towerContainer);
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

            var isSuccess = _towerParams.TryGetTowerContainer(current.ElementalType, out TowerContainer towerContainer);

            if (isSuccess == true)
            {
                InstantiateTower(towerContainer);
            }
            else
            {
                Debug.LogError("Class: 'Builder', Method: 'BuildTower', Message: 'isSuccess != true'");
            }
        }

        private void InstantiateTower(TowerContainer towerContainer)
        {
            _towerContainer = towerContainer;
            
            _currentTower = Instantiate(towerContainer.Tower, _terrainCollider.transform.position, Quaternion.identity);
            _currentTower.Initialize(towerContainer.TowerModel, towerContainer.CastItem);
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
            if (_currentTower == null)
            {
                return;
            }
            
            _canBuild = _mouseFollower.TryGetBuildPosition(mousePosition, out Vector3 buildPosition);

            if (_canBuild == false)
            {
                return;
            }
            
            _currentTower.transform.position = buildPosition;
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
using Game;
using Infrastructure;
using ModalWindows;
using UnityEngine;
using Zenject;

namespace UI.Game
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private BuildPanel _buildPanel;
        [SerializeField] private InfoPanel _infoPanel;
        [SerializeField] private AbilityPanel _abilityPanel;
        [SerializeField] private NotificationsPanel _notificationsPanel;

        private GameMediator _gameMediator;
        private PlayerModel _playerModel;
        private ConfirmationWindowModel _confirmationWindowModel;

        [Inject]
        public void Construction(GameMediator gameMediator, PlayerModel playerModel, ConfirmationWindowModel confirmationWindowModel)
        {
            _gameMediator = gameMediator;
            _playerModel = playerModel;
            _confirmationWindowModel = confirmationWindowModel;

            _playerModel.Health.ValueChanged += ChangeHealthValue;
            _playerModel.Gold.ValueChanged += ChangeGoldValue;
            _playerModel.CurrentBuilding.ValueChanged += ShowInfo;
            _playerModel.Notification.ValueChanged += ShowMessage;
        }

        public void StartClock() => _topPanel.StartClock();
        
        public void PauseClock() => _topPanel.PauseClock();
        
        public void ResetClock() => _topPanel.ResetClock();
        
        public void ShowInfo(BuildModel buildModel) => _infoPanel.ShowInfo(buildModel);

        public void ClearInfo() => _infoPanel.ClearInfo();

        public void ChangeHealthValue(int current, int previous)
        {
            _topPanel.ShowHealth(current);

            if (current <= 0)
            {
                Debug.Log("You've lost");
                _playerModel.Notification = "u lost";
                _gameMediator.StopMatch();
            }
        }

        public void ChangeGoldValue(int current, int previous)
        {
            _topPanel.ShowGold(current);
        }
        
        public void BuildFireTower()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetFireTowerModel();
        }

        public void BuildPoisonTower()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetPoisonTowerModel();
        }

        public void BuildWaterTower()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetWaterTowerModel();
        }

        public void BuildIceTower()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetIceTowerModel();
        }

        private void Start()
        {
            StartClock();

            _topPanel.OnMenuClicked += OpenMenu;

            _buildPanel.OnFireTowerClicked += BuildFireTower;
            _buildPanel.OnPoisonTowerClicked += BuildPoisonTower;
            _buildPanel.OnWaterTowerClicked += BuildWaterTower;
            _buildPanel.OnIceTowerClicked += BuildIceTower;
        
            _topPanel.ShowGold(_playerModel.Gold);
        }

        private void OnDestroy()
        {
            _topPanel.OnMenuClicked -= OpenMenu;

            _buildPanel.OnFireTowerClicked -= BuildFireTower;
            _buildPanel.OnPoisonTowerClicked -= BuildPoisonTower;
            _buildPanel.OnWaterTowerClicked -= BuildWaterTower;
            _buildPanel.OnIceTowerClicked -= BuildIceTower;
            
            _playerModel.Health.ValueChanged -= ChangeHealthValue;
            _playerModel.Gold.ValueChanged -= ChangeGoldValue;
            _playerModel.CurrentBuilding.ValueChanged -= ShowInfo;
            _playerModel.Notification.ValueChanged -= ShowMessage;
        }

        private void ShowMessage(string current, string previous) => _notificationsPanel.ShowMessage(current);

        private void ShowInfo(BuildModel current, BuildModel previous)
        {
            if (current != null)
            {
                ShowInfo(current);
            }
            else
            {
                ClearInfo();
            }
        }

        private void OpenMenu()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            SetConfirmationWindowModel();
            _gameMediator.ShowConfirmationWindow();
        }
        
        private void AcceptDelegate() { _gameMediator.LoadMainMenu(); }

        private void CancelDelegate() { _gameMediator.HideConfirmationWindow(); }

        private void SetConfirmationWindowModel()
        {
            _confirmationWindowModel.Title = "To menu";
            _confirmationWindowModel.Message = "Do you want to go to the menu?";
            _confirmationWindowModel.AcceptLabel = "Yes";
            _confirmationWindowModel.CancelLabel = "No";
            _confirmationWindowModel.AcceptDelegate = AcceptDelegate;
            _confirmationWindowModel.CancelDelegate = CancelDelegate;
        }

        public void Reset()
        {
            ResetClock();
            StartClock();
            ClearInfo();
        }
    }
}
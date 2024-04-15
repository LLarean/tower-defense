using Game;
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

        [Inject]
        public void Construction(GameMediator gameMediator, PlayerModel playerModel)
        {
            _gameMediator = gameMediator;
            _playerModel = playerModel;

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
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetFireTowerModel();
        }

        public void BuildPoisonTower()
        {
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetPoisonTowerModel();
        }

        public void BuildWaterTower()
        {
            _playerModel.CurrentBuilding.Value = KeeperBuildingModels.GetWaterTowerModel();
        }

        public void BuildIceTower()
        {
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
            AcceptDelegate acceptDelegate = () => { _gameMediator.LoadMainMenu(); };
            CancelDelegate cancelDelegate = () => { _gameMediator.HideConfirmationWindow(); };
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel
            {
                Title = "To menu",
                Message = "Do you want to go to the menu?",
                AcceptLabel = "Yes",
                CancelLabel = "No",
                AcceptDelegate = acceptDelegate,
                CancelDelegate = cancelDelegate,
            };
            
            _gameMediator.InitializeConfirmationWindow(confirmationWindowModel);
            _gameMediator.ShowConfirmationWindow();
        }
    }
}
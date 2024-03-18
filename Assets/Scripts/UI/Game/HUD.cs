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

        private void Start()
        {
            StartClock();

            _topPanel.OnMenuClicked += OpenMenu;

            _buildPanel.OnFireTowerClicked += BuildFireTower;
            _buildPanel.OnIceTowerClicked += BuildIceTower;
        
            _topPanel.ShowGold(_playerModel.Gold);
        }

        public void BuildFireTower()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.FireTower,
                CastType = CastType.Fire,
                Damage = GlobalParams.FireTowerDamage,
                Price = GlobalParams.FireTowerPrice,
            };

            _playerModel.CurrentBuilding.Value = buildModel;
        }

        public void BuildIceTower()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.IceTower,
                CastType = CastType.Ice,
                Damage = GlobalParams.IceTowerDamage,
                Price = GlobalParams.IceTowerPrice,
            };
            
            _playerModel.CurrentBuilding.Value = buildModel;
        }

        private void OpenMenu()
        {
        }
    }
}
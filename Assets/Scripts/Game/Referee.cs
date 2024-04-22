using Infrastructure;
using ModalWindows;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Referee : MonoBehaviour, IEnemyHandler, IGameHandler
    {
        private GameMediator _gameMediator;
        private PlayerModel _playerModel;
        private NotificationWindowModel _notificationWindowModel;

        [Inject]
        public void Construction(GameMediator gameMediator, PlayerModel playerModel, NotificationWindowModel notificationWindowModel)
        {
            _gameMediator = gameMediator;
            _playerModel = playerModel;
            _notificationWindowModel = notificationWindowModel;
        }
        
        public void HandleEnemyDestroy()
        {
            _playerModel.Gold.Value += GlobalParams.RewardMurder;
        }

        public void HandleFinishRoute()
        {
            _playerModel.Notification.Value = GlobalStrings.EnemyReached;
            _playerModel.Health.Value -= GlobalParams.DamagePlayer;
        }

        public void HandleFinishMatch()
        {
            FinishMatch();
        }

        private void Start()
        {
            EventBus.Subscribe(this);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(this);
        }

        private void FinishMatch()
        {
            SetNotificationWindowModel();
            _gameMediator.ShowNotificationWindow();
        }

        private void SetNotificationWindowModel()
        {
            _notificationWindowModel.Title = "The end";
            _notificationWindowModel.Message = "The game is over";
            _notificationWindowModel.ConfirmLabel = "To Menu";
            _notificationWindowModel.ConfirmDelegate = ConfirmDelegate;
        }
        
        private void ConfirmDelegate() => _gameMediator.LoadMainMenu();
    }
}

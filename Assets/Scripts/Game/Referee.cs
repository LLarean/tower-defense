using GameUtilities;
using Globals;
using Infrastructure;
using ModalWindows;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game
{
    public class Referee : MonoBehaviour, IEnemyHandler, IGameHandler
    {
        [Inject] private GameMediator _gameMediator;
        [Inject] private PlayerModel _playerModel;
        [Inject] private NotificationWindowModel _notificationWindowModel;

        private int _enemiesCompletedPath;
        
        public void StartMatch()
        {
            _enemiesCompletedPath = 0;

            StartRound();
        }

        public void HandlePrepareRound()
        {
            CustomLogger.Log("Preparing for the match", LogImportance.Low);
        }

        public void HandleStartRound()
        {
            CustomLogger.Log("The round is started", LogImportance.Low);
            _playerModel.Notification.Value = GlobalStrings.RoundStart;
            _enemiesCompletedPath = 0;
        }

        public void HandleStopRound()
        {
            CustomLogger.Log("The round is stopped", LogImportance.Low);
            _playerModel.Notification.Value = GlobalStrings.RoundOver;
        }

        public void HandleDestroy()
        {
            _enemiesCompletedPath++;
            _playerModel.Gold.Value += GlobalParams.RewardMurder;
            FinishRound();
        }

        public void HandleFinishRoute()
        {
            _enemiesCompletedPath++;
            _playerModel.Notification.Value = GlobalStrings.EnemyReached;

            TakeDamage();
            FinishRound();
        }

        private void TakeDamage()
        {
            var healthValue = _playerModel.Health.Value - GlobalParams.DamagePlayer;

            if (healthValue >= 0)
            {
                _playerModel.Health.Value -= GlobalParams.DamagePlayer;
            }
            else
            {
                _playerModel.Health.Value = 0;
            }
        }

        private void FinishRound()
        {
            if (_playerModel.Health.Value <= 0)
            {
                _playerModel.Notification.Value = "End";
                ShowModalWindow();
            }

            bool isSuccess = _gameMediator.TryGetCurrentRoundModel(out RoundModel roundModel);
            
            if (isSuccess == false)
            {
                return;
            }

            if (roundModel.IsInfinite == true)
            {
                _gameMediator.RestartRound();
            }

            if (_enemiesCompletedPath >= roundModel.NumberEnemies)
            {
                StartRound();
            }
        }
        
        private void StartRound()
        {
            bool isStarted = _gameMediator.TryStartRound();

            if (isStarted == false)
            {
                ShowModalWindow();
            }
        }

        private void Start()
        {
            EventBus.Subscribe(this);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(this);
        }

        private void ShowModalWindow()
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
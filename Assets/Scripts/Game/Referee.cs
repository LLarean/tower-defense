using GameUtilities;
using Infrastructure;
using ModalWindows;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game
{
    public class Referee : MonoBehaviour, IEnemyHandler, IGameHandler
    {
        [SerializeField] private RoundStarter _roundStarter;
        [SerializeField] private MatchSettings _matchSettings;

        private GameMediator _gameMediator;
        private PlayerModel _playerModel;
        private NotificationWindowModel _notificationWindowModel;

        private int _enemiesCompletedPath;
        
        [Inject]
        public void Construction(GameMediator gameMediator, PlayerModel playerModel, NotificationWindowModel notificationWindowModel)
        {
            _gameMediator = gameMediator;
            _playerModel = playerModel;
            _notificationWindowModel = notificationWindowModel;
        }

        public void StartMatch()
        {
            _enemiesCompletedPath = 0;

            StartRound();
        }

        public void HandlePrepareRound()
        {
            CustomLogger.Log("Preparing for the match", 2);
            // TODO Display the preparation time
        }

        public void HandleStartRound()
        {
            CustomLogger.Log("The round is started", 2);
            _playerModel.Notification.Value = GlobalStrings.RoundStart;
        }

        public void HandleStopRound()
        {
            CustomLogger.Log("The round is stopped", 2);

            _playerModel.Notification.Value = GlobalStrings.RoundOver;
        }

        public void HandleFinishMatch()
        {
            _playerModel.Notification.Value = "End";
            ShowModalWindow();
        }

        public void HandleDestroy()
        {
            _enemiesCompletedPath++;
            _playerModel.Gold.Value += GlobalParams.RewardMurder;
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
                EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleFinishMatch());
            }

            bool isSuccess = _matchSettings.TryGetCurrentRoundModel(out RoundModel roundModel);
            
            if (isSuccess == false)
            {
                return;
            }

            if (roundModel.IsInfinite == true)
            {
                _roundStarter.RestartRound();
            }

            if (_enemiesCompletedPath >= roundModel.NumberEnemies)
            {
                StartRound();
            }
        }
        
        private void StartRound()
        {
            bool isStarted = _roundStarter.TryStartRound();

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
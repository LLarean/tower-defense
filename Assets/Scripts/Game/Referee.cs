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
        [Inject] private ConfirmationWindowModel _confirmationWindowModel;

        private int _enemiesCompletedPath;
        
        public void StartMatch()
        {
            _enemiesCompletedPath = 0;

            StartRound();
        }
        
        public void ResetMatch()
        {
            CustomLogger.Log("The match will be restarted", LogPriority.Low);
            
            // TODO It needs to be reworked
            _enemiesCompletedPath = 0;
            _playerModel.Gold.Value = 150;
            _playerModel.Health.Value = 100;
        }


        public void HandlePrepareRound()
        {
            CustomLogger.Log("Preparing for the match", LogPriority.Low);
        }

        public void HandleStartRound()
        {
            CustomLogger.Log("The round is started", LogPriority.Low);
            _playerModel.Notification.Value = GlobalStrings.RoundStart;
            _enemiesCompletedPath = 0;
        }

        public void HandleStopRound()
        {
            CustomLogger.Log("The round is stopped", LogPriority.Low);
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
            SetСonfirmationWindowModel();
            
            _gameMediator.ShowConfirmationWindow();
        }

        private void SetСonfirmationWindowModel()
        {
            _confirmationWindowModel.Title = "The end";
            _confirmationWindowModel.Message = "Restart the match?";
            _confirmationWindowModel.AcceptLabel = "Restart";
            _confirmationWindowModel.CancelLabel = "To Menu";
            _confirmationWindowModel.AcceptDelegate = AcceptDelegate;
            _confirmationWindowModel.CancelDelegate = CancelDelegate;
        }

        private void AcceptDelegate() => _gameMediator.RestartMatch();
        private void CancelDelegate() => _gameMediator.LoadMainMenu();
    }
}
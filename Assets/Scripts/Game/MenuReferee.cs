using Infrastructure;
using Menu;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game
{
    public class MenuReferee : MonoBehaviour, IGameHandler, IEnemyHandler
    {
        [Inject] private MenuMediator _menuMediator;
        [Inject] private MatchSettings _matchSettings;

        public void StartMatch()
        {
            CustomLogger.Log("We are trying to start the match", LogPriority.Low);
            _menuMediator.StartRound();
        }
        
        public void HandlePrepareRound()
        {
            CustomLogger.Log("Preparations are underway to launch the round", LogPriority.Low);
        }

        public void HandleStartRound()
        {
            CustomLogger.Log("The round has started", LogPriority.Low);
        }

        public void HandleStopRound()
        {
            CustomLogger.Log("The round has stopped", LogPriority.Low);
        }

        public void HandleDestroy()
        {
            CustomLogger.Log("The enemy is destroyed", LogPriority.Low);
            FinishRound();
        }

        public void HandleFinishRoute()
        {
            CustomLogger.Log("The enemy has completed his route", LogPriority.Low);
            FinishRound();
        }

        private void FinishRound()
        {
            bool isSuccess = _matchSettings.TryGetCurrentRoundModel(out RoundModel roundModel);
            
            if (isSuccess == false)
            {
                return;
            }

            if (roundModel.IsInfinite == true)
            {
                CustomLogger.Log("We are trying to restart the round", LogPriority.Low);
                _menuMediator.RestartRound();
            }
        }

        private void Start() => EventBus.Subscribe(this);

        private void OnDestroy() => EventBus.Unsubscribe(this);
    }
}
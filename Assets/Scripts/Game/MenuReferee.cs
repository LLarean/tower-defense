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
            CustomLogger.Log("We are trying to start the match", LogImportance.Low);
            _menuMediator.StartRound();
        }
        
        public void HandlePrepareRound()
        {
            CustomLogger.Log("Preparations are underway to launch the round", LogImportance.Low);
        }

        public void HandleStartRound()
        {
            CustomLogger.Log("The round has started", LogImportance.Low);
        }

        public void HandleStopRound()
        {
            CustomLogger.Log("The round has stopped", LogImportance.Low);
        }

        public void HandleDestroy()
        {
            CustomLogger.Log("The enemy is destroyed", LogImportance.Low);
            FinishRound();
        }

        public void HandleFinishRoute()
        {
            CustomLogger.Log("The enemy has completed his route", LogImportance.Low);
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
                CustomLogger.Log("The round has been restarted", LogImportance.Low);
                _menuMediator.RestartRound();
            }
        }

        private void Start() => EventBus.Subscribe(this);

        private void OnDestroy() => EventBus.Unsubscribe(this);
    }
}
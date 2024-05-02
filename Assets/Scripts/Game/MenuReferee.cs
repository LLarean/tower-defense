using Infrastructure;
using Menu;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game
{
    public class MenuReferee : MonoBehaviour, IEnemyHandler
    {
        [Inject] private MenuMediator _menuMediator;
        [Inject] private MatchSettings _matchSettings;

        public void StartMatch()
        {
            CustomLogger.Log("The match is started", 2);
            _menuMediator.StartRound();
        }

        public void HandleDestroy()
        {
            CustomLogger.Log("The enemy is destroyed", 2);
            FinishRound();
        }

        public void HandleFinishRoute()
        {
            CustomLogger.Log("The enemy has completed his route", 2);
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
                CustomLogger.Log("The round has been restarted", 2);
                _menuMediator.RestartRound();
            }
        }

        private void Start() => EventBus.Subscribe(this);

        private void OnDestroy() => EventBus.Unsubscribe(this);
    }
}
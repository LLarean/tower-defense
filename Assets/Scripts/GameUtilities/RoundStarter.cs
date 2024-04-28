using System.Collections;
using GameLogic.Navigation;
using Infrastructure;
using UnityEngine;
using Utilities;
using Zenject;

namespace GameUtilities
{
    public class RoundStarter : MonoBehaviour, IEnemyHandler
    {
        [SerializeField] private EnemiesRouter _enemiesRouter;
        [SerializeField] private MatchModel _matchModel;

        private PlayerModel _playerModel;
        private Coroutine _coroutine;
        
        private int _currentRoundIndex;
        private int _numberEnemiesDestroyed;

        [Inject]
        public void Construction(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }
        
        public void StartRound()
        {
            CustomLogger.Log("The round is started", 2);
            
            EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleStartRound());
            
            _coroutine = StartCoroutine(DelayingRoutingEnemies());
        }

        public void StopMatch()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _currentRoundIndex = 0;
            _numberEnemiesDestroyed = 0;
            CustomLogger.Log("The match is stopped", 2);
        }

        public void HandleNavigationPointVisit()
        {
            throw new System.NotImplementedException();
        }

        public void HandleEnemyDestroy()
        {
            CustomLogger.Log("The tower destroyed the enemy", 2);
            _numberEnemiesDestroyed++;
            
            if (_matchModel.RoundSettings[^1].IsInfinite == true)
            {
                return;
            }

            if (_currentRoundIndex >= _matchModel.RoundSettings.Count)
            {
                EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleFinishMatch());
                return;
            }
            
            if (_numberEnemiesDestroyed >= _matchModel.RoundSettings[_currentRoundIndex].NumberEnemies)
            {
                StartRound();
            }
        }

        public void HandleFinishRoute()
        {
            CustomLogger.Log("The enemy has reached the end", 2);
            _numberEnemiesDestroyed++;
            
            if (_matchModel.RoundSettings[^1].IsInfinite == true)
            {
                return;
            }

            if (_currentRoundIndex >= _matchModel.RoundSettings.Count)
            {
                EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleFinishMatch());
                return;
            }

            if (_numberEnemiesDestroyed >= _matchModel.RoundSettings[_currentRoundIndex].NumberEnemies)
            {
                StartRound();
            }
        }

        private void Start()
        {
            EventBus.Subscribe(this);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(this);
            StopMatch();
        }

        private IEnumerator DelayingRoutingEnemies()
        {
            _numberEnemiesDestroyed = 0;

            // TODO updating the seconds in the message
            _playerModel.Notification.Value = $"{GlobalStrings.RoundWillStart} {_matchModel.RoundStartDelay} {GlobalStrings.Seconds}";
            yield return new WaitForSeconds(_matchModel.RoundStartDelay);
            
            _enemiesRouter.StartRouting(_matchModel.RoundSettings[_currentRoundIndex]);
            _currentRoundIndex++;
        }
    }
}
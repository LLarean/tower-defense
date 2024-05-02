using System.Collections;
using Game;
using GameLogic.Navigation;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace GameUtilities
{
    public class RoundStarter : MonoBehaviour
    {
        [Inject] private EnemiesCreator _enemiesCreator;
        [Inject] private EnemiesRouter _enemiesRouter;
        [Inject] private MatchSettings _matchSettings;

        private RoundModel _roundModel;
        private Coroutine _coroutine;

        public void RestartRound()
        {
            _enemiesCreator.Reset();
            _coroutine = StartCoroutine(PreparingForRound());
        }
        
        public bool TryStartRound()
        {
            var isStarted = false;

            bool isReceived = _matchSettings.TryGetNextRoundModel(out RoundModel roundModel);

            if (isReceived == false)
            {
                return isStarted;
            }

            _roundModel = roundModel;
            CreateEnemies();
            _coroutine = StartCoroutine(PreparingForRound());
            isStarted = true;
            
            return isStarted;
        }

        public void StopMatch()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleStopRound());
        }

        private void OnDestroy()
        {
            StopMatch();
        }

        private IEnumerator PreparingForRound()
        {
            EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandlePrepareRound());
            
            yield return new WaitForSeconds(_matchSettings.RoundStartDelay);
            StartEnemiesRouting();
            EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleStartRound());
        }

        private void CreateEnemies()
        {
            var enemy = _roundModel.Enemy;
            var numberEnemies = _roundModel.NumberEnemies;

            _enemiesCreator.Create(enemy, numberEnemies);
        }
        
        private void StartEnemiesRouting()
        {
            var enemies = _enemiesCreator.Enemies;
            var routingDelaySeconds = _roundModel.EnemySpawnDelay;
            
            _enemiesRouter.StartRouting(enemies, routingDelaySeconds);
        }
    }
}
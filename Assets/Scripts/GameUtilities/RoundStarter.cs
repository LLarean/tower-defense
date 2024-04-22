using System;
using System.Collections;
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
        private int _nextRoundIndex;
        private int _numberDestroyedEnemies;
        private Coroutine _coroutine;

        [Inject]
        public void Construction(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void StartMatch()
        {
            CustomLogger.Log("The match is started", 2);

            if (_matchModel == null)
            {
                CustomLogger.LogError("_matchModel == null");
                return;
            }
            
            _coroutine = StartCoroutine(Waiting(_matchModel.RoundStartDelay));
        }

        public void StopMatch()
        {
            CustomLogger.Log("The match is stopped", 2);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _nextRoundIndex = Int32.MaxValue;
            _numberDestroyedEnemies = Int32.MaxValue;
        }

        public void HandleEnemyDestroy()
        {
            CustomLogger.Log("The tower destroyed the enemy", 2);

            // _playerModel.Gold.Value += GlobalParams.RewardMurder;
            DestroyUnit();
        }

        public void HandleFinishRoute()
        {
            CustomLogger.Log("The enemy has reached the end", 2);

            _playerModel.Notification.Value = GlobalStrings.EnemyReached;
            // _playerModel.Health.Value -= GlobalParams.DamagePlayer;

            DestroyUnit();
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

        private void DestroyUnit()
        {
            _numberDestroyedEnemies++;
            var currentRound = _nextRoundIndex - 1;

            if (_numberDestroyedEnemies < _matchModel.RoundSettings[currentRound].NumberEnemies)
            {
                return;
            }

            if (_matchModel.RoundSettings[currentRound].IsInfinite == true)
            {
                return;
            }

            CustomLogger.Log("The round is over", 2);

            _playerModel.Notification.Value = GlobalStrings.RoundOver;
            StartCoroutine(Waiting(_matchModel.RoundStartDelay));
        }

        private IEnumerator Waiting(float waitingTime)
        {
            // TODO updating the seconds in the message
            _playerModel.Notification.Value = $"{GlobalStrings.RoundWillStart} {waitingTime} {GlobalStrings.Seconds}";
            yield return new WaitForSeconds(waitingTime);
            StartRound();
        }

        private void FinishMatch()
        {
            CustomLogger.Log("FinishMatch", 3);
            
            EventBus.RaiseEvent<IGameHandler>(gameHandler => gameHandler.HandleFinishMatch());

            _playerModel.Notification.Value = "End";
        }

        private void StartRound()
        {
            if (_nextRoundIndex < _matchModel.RoundSettings.Count)
            {
                StartNewRound();
            }
            else
            {
                FinishMatch();
            }
        }

        private void StartNewRound()
        {
            CustomLogger.Log("The round is started", 2);
            _numberDestroyedEnemies = 0;
            _playerModel.Notification.Value = GlobalStrings.RoundStart;

            _enemiesRouter.StartRouting(_matchModel.RoundSettings[_nextRoundIndex]);
            _nextRoundIndex++;
        }
    }
}
using System;
using System.Collections;
using Infrastructure;
using UnityEngine;
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
            Debug.Log("The match is started");
            _coroutine = StartCoroutine(Waiting(_matchModel.RoundStartDelay));
        }

        public void StopMatch()
        {
            Debug.Log("The match is stopped");

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _nextRoundIndex = Int32.MaxValue;
            _numberDestroyedEnemies = Int32.MaxValue;
        }

        public void HandleDestroy()
        {
            Debug.Log("The tower destroyed the enemy");
            _playerModel.Gold.Value += GlobalParams.RewardMurder;
            DestroyUnit();
        }

        public void HandleFinish()
        {
            Debug.Log("The enemy has reached the end");
            _playerModel.Notification.Value = GlobalStrings.EnemyReached;
            _playerModel.Health.Value -= GlobalParams.DamagePlayer;

            DestroyUnit();
        }

        protected virtual void FinishMatch()
        {
            Debug.Log("Don't start");
            _playerModel.Notification.Value = "End";
        }

        private void Start()
        {
            EventBus.Subscribe(this);
            StartMatch();
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

            Debug.Log("The round is over");
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
            Debug.Log("The round is started");
            _numberDestroyedEnemies = 0;
            _playerModel.Notification.Value = GlobalStrings.RoundStart;

            _enemiesRouter.StartRouting(_matchModel.RoundSettings[_nextRoundIndex]);
            _nextRoundIndex++;
        }
    }
}
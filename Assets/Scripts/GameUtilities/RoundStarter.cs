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

        public event Action MatchIsFinished;

        [Inject]
        public void Construction(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void StartMatch()
        {
            CustomLogger.LogMessage("The match is started", 3);

            _coroutine = StartCoroutine(Waiting(_matchModel.RoundStartDelay));
        }

        public void StopMatch()
        {
            CustomLogger.LogMessage("The match is stopped", 3);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _nextRoundIndex = Int32.MaxValue;
            _numberDestroyedEnemies = Int32.MaxValue;
        }

        public void HandleDestroy()
        {
            CustomLogger.LogMessage("The tower destroyed the enemy", 3);

            _playerModel.Gold.Value += GlobalParams.RewardMurder;
            DestroyUnit();
        }

        public void HandleFinish()
        {
            CustomLogger.LogMessage("The enemy has reached the end", 3);

            _playerModel.Notification.Value = GlobalStrings.EnemyReached;
            _playerModel.Health.Value -= GlobalParams.DamagePlayer;

            DestroyUnit();
        }

        protected virtual void FinishMatch()
        {
            CustomLogger.LogMessage("Don't start", 3);
            _playerModel.Notification.Value = "End";
            MatchIsFinished?.Invoke();
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

            CustomLogger.LogMessage("The round is over", 3);

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

        protected virtual void StartRound()
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
            CustomLogger.LogMessage("The round is started", 3);
            _numberDestroyedEnemies = 0;
            _playerModel.Notification.Value = GlobalStrings.RoundStart;

            _enemiesRouter.StartRouting(_matchModel.RoundSettings[_nextRoundIndex]);
            _nextRoundIndex++;
        }
    }
}
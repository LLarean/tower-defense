using System;
using System.Collections;
using Infrastructure;
using UnityEngine;
using Zenject;

public class RoundStarter : MonoBehaviour, IEnemyHandler
{
    [SerializeField] private EnemiesRouter _enemiesRouter;
    [SerializeField] private MatchModel _matchModel;
    
    private PlayerModel _playerModel;
    
    private int _currentRoundIndex;
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
        
        _currentRoundIndex = Int32.MaxValue;
        _numberDestroyedEnemies = Int32.MaxValue;
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
        _playerModel.Gold.Value += GlobalParams.RewardMurder;

        // TODO rename?
        var index = _currentRoundIndex - 1;
        
        if (_numberDestroyedEnemies < _matchModel.RoundSettings[index].NumberEnemies)
        {
            return;
        }
        
        if (_matchModel.RoundSettings[index].IsInfinite == true)
        {
            return;
        }

        Debug.Log("The round is over");
        _playerModel.Notification.Value = GlobalStrings.RoundOver;
        
        StartCoroutine(Waiting(_matchModel.RoundStartDelay));
    }

    private IEnumerator Waiting(float waitingTime)
    {
        _playerModel.Notification.Value = $"{GlobalStrings.RoundWillStart} {waitingTime} {GlobalStrings.Seconds}";
        yield return new WaitForSeconds(waitingTime);
        StartRound();
    }

    private void StartRound()
    {
        if (_currentRoundIndex < _matchModel.RoundSettings.Count)
        {
            Debug.Log("The round is started");
            _numberDestroyedEnemies = 0;
            _playerModel.Notification.Value = GlobalStrings.RoundStart;

            _enemiesRouter.StartRouting(_matchModel.RoundSettings[_currentRoundIndex]);
            _currentRoundIndex++;
        }
        else
        {
            Debug.Log("Don't start");
            _playerModel.Notification.Value = "End";
        }
    }

    public void HandleDestroy()
    {
        Debug.Log("The tower destroyed the enemy");
        DestroyUnit();
    }

    public void HandleFinish()
    {
        Debug.Log("The enemy has reached the end");
        _playerModel.Notification.Value = GlobalStrings.EnemyReached;
        _playerModel.Health.Value -= GlobalParams.DamagePlayer;
        
        DestroyUnit();
    }
}
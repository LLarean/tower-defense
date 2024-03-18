using System;
using System.Collections;
using EventBusSystem;
using UnityEngine;
using Zenject;

public class RoundStarter : MonoBehaviour, IEnemyHandler
{
    [SerializeField] private MatchModel _matchModel;
    
    private EnemiesSpawner _enemiesSpawner;
    private PlayerModel _playerModel;
    
    private int _currentRoundIndex;
    private int _numberDestroyedEnemies;

    [Inject]
    public void Construction(EnemiesSpawner enemiesSpawner, PlayerModel playerModel)
    {
        _enemiesSpawner = enemiesSpawner;
        _playerModel = playerModel;
    }

    public void StartMatch()
    {
        StartCoroutine(Waiting(_matchModel.RoundDelay));
    }

    private void Start()
    {
        EventBus.Subscribe(this);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(this);
    }

    private void DestroyUnit()
    {
        _numberDestroyedEnemies++;
        _playerModel.Gold.Value += GlobalParams.RewardMurder;
        
        if (_numberDestroyedEnemies < _matchModel.RoundSettings[_currentRoundIndex].NumberEnemies - 1)
        {
            return;
        }

        StartCoroutine(Waiting(_matchModel.RoundDelay));
    }

    private IEnumerator Waiting(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        StartRound();
    }

    private void StartRound()
    {
        if (_currentRoundIndex < _matchModel.RoundSettings.Count)
        {
            _enemiesSpawner.StartRound(_matchModel.RoundSettings[_currentRoundIndex]);
            _currentRoundIndex++;
        }
        else
        {
            Debug.Log("WON");
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
        DestroyUnit();
    }
}
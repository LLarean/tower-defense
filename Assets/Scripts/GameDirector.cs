using UnityEngine;
using Zenject;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private GameObject _tempWindow;
    
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _numberRounds;
    [SerializeField] private int _numberEnemies;
    
    private EnemiesSpawner _enemiesSpawner;
    
    private int _currentRound;
    private int _numberDestroyedEnemies;
    private PlayerModel _playerModel;

    [Inject]
    public void Construction(EnemiesSpawner enemiesSpawner, PlayerModel playerModel)
    {
        _enemiesSpawner = enemiesSpawner;
        _playerModel = playerModel;
        
        enemiesSpawner.OnDestroyed += AddGold;
    }

    private void AddGold()
    {
        _numberDestroyedEnemies++;
        _playerModel.Gold.Value += GlobalParams.RewardMurder;

        FinishRound();
    }

    public void StartMatch()
    {
        StartRound();
    }
    
    private void DestroyUnit()
    {
        _numberDestroyedEnemies++;
        
        if (_numberDestroyedEnemies < _numberEnemies)
        {
            return;
        }

        if (_currentRound < _numberRounds)
        {
            StartRound();
        }
        
        // ShowWonWindow();
    }

    private void StartRound()
    {
        _currentRound++;
        _enemiesSpawner.StartRound(_numberEnemies, _enemy);
    }

    private void FinishRound()
    {
        if (_numberDestroyedEnemies >= _numberEnemies)
        {
            _tempWindow.gameObject.SetActive(true);
        }
    }
}
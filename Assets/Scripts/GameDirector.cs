using UnityEngine;
using Zenject;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _numberRounds;
    [SerializeField] private int _numberEnemies;
    
    private EnemiesSpawner _enemiesSpawner;
    
    private int _currentRound;
    private int _numberDestroyedEnemies;
    private HUD _hud;

    [Inject]
    public void Construction(EnemiesSpawner enemiesSpawner, HUD hud)
    {
        _enemiesSpawner = enemiesSpawner;
        _hud = hud;
        
        enemiesSpawner.OnDestroyed += AddGold;
    }

    private void AddGold()
    {
        _hud.AddGold();
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
}
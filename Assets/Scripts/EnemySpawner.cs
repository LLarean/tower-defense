using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _startPoint; 
    [SerializeField] private Transform _finishPoint; 
        
    [Header("Units")]
    [SerializeField] private Enemy _enemy;

    private List<Enemy> _enemies = new List<Enemy>();

    private int _maximumEnemies = 5; 
    private int _numberEnemiesCreated;
    private float _spawnDelay = 2f; 
    private int _damage = 1;
    private int _costKilling = 10;

    public void StartSpawn()
    {
        StartCoroutine(CreatingEnemies());
    }

    public void FinishSpawn()
    {
    }
        
    public void Finish()
    {
    }

    public void DeleteUnit(Enemy enemy) => _enemies.Remove(enemy);

    private void Start()
    {
        StartSpawn();
    }

    private IEnumerator CreatingEnemies()
    {
        while (_numberEnemiesCreated < _maximumEnemies)
        {
            _numberEnemiesCreated++;
                
            Enemy enemy = Instantiate(_enemy, _startPoint.position, Quaternion.identity);
            enemy.Init(this, _finishPoint);
            _enemies.Add(enemy);
                
            yield return new WaitForSeconds(_spawnDelay);
        }
            
        StartCoroutine(EndWait());
    }
        
    private IEnumerator EndWait()
    {
        while (_enemies.Count != 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);
    }
}

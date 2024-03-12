using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint; 
    [SerializeField] private Transform _destroyPoint; 
    [SerializeField] private List<Transform> _wayPoints; 
        
    [Header("Units")]
    [SerializeField] private Enemy _enemy;

    private List<Enemy> _enemies = new List<Enemy>();

    private int _maximumEnemies = 5;
    private int _numberEnemiesCreated;
    private float _spawnDelay = 2f;

    public void StartSpawn()
    {
        StartCoroutine(CreateEnemies());
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

    private IEnumerator CreateEnemies()
    {
        while (_numberEnemiesCreated < _maximumEnemies)
        {
            _numberEnemiesCreated++;
                
            Enemy enemy = Instantiate(_enemy, _spawnPoint.position, Quaternion.identity);
            enemy.Initialize(_wayPoints, _destroyPoint);
            _enemies.Add(enemy);
                
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
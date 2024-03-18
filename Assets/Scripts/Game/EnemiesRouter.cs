using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRouter : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint; 
    [SerializeField] private Transform _destroyPoint; 
    [SerializeField] private List<Transform> _wayPoints;

    private RoundModel _roundModel;
    private int _numberEnemiesCreated;

    public void StartRound(RoundModel roundModel)
    {
        _roundModel = roundModel;
        _numberEnemiesCreated = 0;
        StartCoroutine(CreatingEnemies());
    }

    private IEnumerator CreatingEnemies()
    {
        while (_numberEnemiesCreated < _roundModel.NumberEnemies)
        {
            Enemy enemy = Instantiate(_roundModel.Enemy, _spawnPoint.position, Quaternion.identity);
            enemy.Initialize(_wayPoints, _destroyPoint);
            _numberEnemiesCreated++;
            yield return new WaitForSeconds(_roundModel.SpawnDelay);
        }
    }
}
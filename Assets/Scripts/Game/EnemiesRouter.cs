using System.Collections;
using UnityEngine;

public class EnemiesRouter : MonoBehaviour
{
    [SerializeField] private PathModel _pathModel;
    
    private int _numberEnemiesCreated;
    private Coroutine _coroutine;

    public void StartRouting(RoundModel roundModel)
    {
        if (roundModel == null)
        {
            Debug.LogError("Class: 'EnemiesRouter', Method: 'StartRoute', Message: 'roundModel == null'");
            return;
        }
        
        if (roundModel.Enemy == null)
        {
            Debug.LogError("Class: 'EnemiesRouter', Method: 'StartRoute', Message: 'roundModel.Enemy == null'");
            return;
        }
        
        _coroutine = StartCoroutine(CreatingEnemies(roundModel));
    }

    private IEnumerator CreatingEnemies(RoundModel roundModel)
    {
        _numberEnemiesCreated = 0;
        
        while (roundModel.IsInfinite == true || _numberEnemiesCreated < roundModel.NumberEnemies)
        {
            Enemy enemy = Instantiate(roundModel.Enemy, _pathModel.SpawnPoint.position, Quaternion.identity);
            enemy.Initialize(_pathModel);
            _numberEnemiesCreated++;
            
            yield return new WaitForSeconds(roundModel.EnemySpawnDelay);
        }
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
}
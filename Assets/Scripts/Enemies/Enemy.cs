using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Init(EnemySpawner enemySpawner, Transform finishPoint)
    {
        gameObject.transform.DOMove(finishPoint.position, 5);
    }
}

using Infrastructure;
using Units;
using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        var isAvailable = collision.TryGetComponent<Enemy>(out Enemy enemy);

        if (isAvailable == false)
        {
            return;
        }
        
        EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleFinish());
        Destroy(enemy.gameObject);
    }
}
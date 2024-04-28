using Infrastructure;
using Units;
using UnityEngine;

namespace GameLogic.EnemyNavigation
{
    public class WayPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collision)
        {
            var isAvailable = collision.TryGetComponent(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }
        
            EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleNavigationPointVisit());
        }
    }
}
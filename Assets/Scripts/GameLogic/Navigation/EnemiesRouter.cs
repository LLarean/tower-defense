using System.Collections;
using System.Collections.Generic;
using GameLogic.EnemyNavigation;
using Infrastructure;
using Units;
using UnityEngine;

namespace GameLogic.Navigation
{
    public class EnemiesRouter : MonoBehaviour
    {
        [SerializeField] private List<WayPoint> _wayPoints;
    
        private List<Enemy> _enemies;
        private int _routingDelaySeconds;
        private Coroutine _coroutine;
        
        public void StartRouting(List<Enemy> enemies, int routingDelaySeconds)
        {
            _enemies = enemies;
            _routingDelaySeconds = routingDelaySeconds;
            
            StartCoroutine(RoutingEnemies());
        }

        private IEnumerator RoutingEnemies()
        {
            foreach (var enemy in _enemies)
            {
                SetStartingState(enemy);
                enemy.SetWayPoint(_wayPoints[1].transform.position);
                yield return new WaitForSeconds(_routingDelaySeconds);
            }
        }

        private void SetStartingState(Enemy enemy)
        {
            var startPosition = _wayPoints[0].transform.position;
            enemy.transform.position = startPosition;
            enemy.gameObject.SetActive(true);
        }

        private void SetNewWayPoint(Enemy enemy, int wayPointIndex)
        {
            if (wayPointIndex < _wayPoints.Count - 1)
            {
                var newWayPoint = _wayPoints[wayPointIndex + 1];
                enemy.SetWayPoint(newWayPoint.transform.position);
            }
            else
            {
                enemy.gameObject.SetActive(false);
                EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleFinishRoute());
            }
        }

        private void Start()
        {
            for (int i = 0; i < _wayPoints.Count; i++)
            {
                var wayPointIndex = i;
                _wayPoints[i].OnVisited += delegate(Enemy enemy) { SetNewWayPoint(enemy, wayPointIndex); };
            }
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            // TODO recycle subscriptions
            for (int i = 0; i < _wayPoints.Count; i++)
            {
                var wayPointIndex = i;
                _wayPoints[i].OnVisited -= delegate(Enemy enemy) { SetNewWayPoint(enemy, wayPointIndex); };
            }
        }
    }
}
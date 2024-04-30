using System;
using Units;
using UnityEngine;

namespace GameLogic.EnemyNavigation
{
    public class WayPoint : MonoBehaviour
    {
        [SerializeField] private WayPointType _type;
        [SerializeField] private GameObject _teleport;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public event Action<Enemy> OnVisited;
        
        private void OnTriggerEnter(Collider collision)
        {
            var isAvailable = collision.TryGetComponent(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }
        
            OnVisited?.Invoke(enemy);
        }

        private void Start()
        {
            _meshRenderer.enabled = false;

            if (_type != WayPointType.Default)
            {
                _teleport.gameObject.SetActive(true);
            }
            else
            {
                _teleport.gameObject.SetActive(false);
            }
        }
    }
}
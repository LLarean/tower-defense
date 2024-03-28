using Units;
using UnityEngine;

namespace Builds
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private SpellCaster _spellCaster;
        [SerializeField] private bool _isBuilt;
        
        private TowerModel _towerModel;

        public bool IsBuilt => _isBuilt;

        public void Initialize(TowerModel towerModel, CastItem castItem)
        {
            if (towerModel == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'Initialize', Message: 'towerModel == null'");
                return;
            }
            
            _towerModel = towerModel;
            
            if (castItem == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'Initialize', Message: 'castItem == null'");
                return;
            }
            
            _spellCaster.Initialize(towerModel, castItem);
        }
        
        public void DisableConstructionMode() => _isBuilt = true;

        private void OnTriggerEnter(Collider collision)
        {
            if (_isBuilt == false)
            {
                return;
            }
            
            var isAvailable = collision.TryGetComponent(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }

            _spellCaster.SetTarget(enemy.gameObject.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isBuilt == false)
            {
                return;
            }
            
            _spellCaster.ResetTarget();
        }
    }
}
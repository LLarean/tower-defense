using Units;
using UnityEngine;

namespace Builds
{
    public class Tower : Building
    {
        [SerializeField] private SpellCaster _spellCaster;
        
        private TowerModel _towerModel;

        public TowerModel TowerModel => _towerModel;
        public ElementalType ElementalType => _towerModel.ElementalType;

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

        public void SetCastItem(CastItem castItem)
        {
            if (castItem == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'SetCastItem', Message: 'castItem == null'");
                return;
            }
            
            if (_spellCaster == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'Start', Message: '_spellCaster == null'");
                return;
            }
            

        }

        private void OnTriggerEnter(Collider collision)
        {
            if (IsBuilt == false)
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
            if (IsBuilt == false)
            {
                return;
            }
            
            _spellCaster.ResetTarget();
        }
    }
}
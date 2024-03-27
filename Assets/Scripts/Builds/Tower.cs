using Units;
using UnityEngine;

namespace Builds
{
    public class Tower : Building
    {
        [SerializeField] private TowerModel _towerModel;
        [SerializeField] private SpellCaster _spellCaster;

        public ElementalType ElementalType => _towerModel.ElementalType;

        private void Start() 
        {
            if (_spellCaster == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'Start', Message: '_spellCaster == null'");
                return;
            }
            
            _spellCaster.Initialize(_towerModel);
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
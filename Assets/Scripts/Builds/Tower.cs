using Units;
using UnityEngine;

namespace Builds
{
    public class Tower : Building
    {
        [SerializeField] private SpellCaster _spellCaster;

        public CastItemModel CastItemModel => _spellCaster.CastItemModel;

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

            _spellCaster.SetTarget(enemy.gameObject);
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
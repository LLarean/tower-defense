using UnityEngine;

namespace Builds
{
    public class Tower : Building
    {
        [SerializeField] private SpellCaster _spellCaster;

        private void OnTriggerEnter(Collider collision)
        {
            var isAvailable = collision.TryGetComponent(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }

            _spellCaster.SetTarget(enemy.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            _spellCaster.ResetTarget();
        }
    }
}
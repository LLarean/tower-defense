using UnityEngine;

namespace Builds
{
    public class Tower : Building
    {
        [SerializeField] private Type _type;
        [SerializeField] private Missile _missile;

        private void OnTriggerEnter(Collider collision)
        {
            var isAvailable = collision.TryGetComponent<Enemy>(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }

            var missile = Instantiate(_missile, transform.position, Quaternion.identity);
            missile.Init(collision.gameObject.transform);
        }
    }
}
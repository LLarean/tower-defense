using System.Collections;
using UnityEngine;

namespace Builds
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private Transform _castSpawn;
        [SerializeField] private CastItem _castItem;
        [SerializeField] [Range(0, 10)] private float _attackSpeed;

        private Coroutine _coroutine;
        private GameObject _target;
        private bool _canCast;
        
        public CastItemModel CastItemModel => _castItem.CastItemModel;
        
        public void SetTarget(GameObject target)
        {
            if (_target == null)
            {
                _target = target;
                _canCast = true;

                _coroutine = StartCoroutine(Casting());
            }
        }

        public void ResetTarget()
        {
            _target = null;
            _canCast = false;
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        public void StartCasting() => _canCast = true;

        public void PauseCasting() => _canCast = false;

        private IEnumerator Casting()
        {
            while (_canCast == true && _target != null)
            {
                // TODO you need to use the object pool and add checks
                var missile = Instantiate(_castItem, _castSpawn.position, Quaternion.identity);
                missile.Initialize(_target.transform);
                yield return new WaitForSeconds(_attackSpeed);
            }
        }
    }
}
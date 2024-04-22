using System.Collections;
using Infrastructure;
using UnityEngine;
using Utilities;

namespace Builds
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private CastItem _castItem;
        
        private float _attackSpeed;
        private bool _canCast;
        private Transform _target;
        
        private Coroutine _coroutine;

        public void Initialize(float attackSpeed, CastItemModel castItemModel)
        {
            _attackSpeed = attackSpeed;
            
            if (_castItem == null)
            {
                CustomLogger.LogError("_castItem == null");
                return;
            }

            _castItem.Initialize(castItemModel);
        }
        
        public void SetTarget(Transform target)
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

        private IEnumerator Casting()
        {
            while (_canCast == true && _target != null)
            {
                // TODO need to use the object pool and add checks
                var castItem = Instantiate(_castItem, transform.position, Quaternion.identity);
                castItem.Initialize(_castItem.CastItemModel);
                castItem.SetTarget(_target);
                
                EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleCast());
                yield return new WaitForSeconds(_attackSpeed);
            }
        }
    }
}
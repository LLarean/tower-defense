using System.Collections;
using UnityEngine;

namespace Builds
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private Transform _castSpawn;
        
        private TowerModel _towerModel;
        private CastItem _castItem;
        
        private Transform _target;
        private Coroutine _coroutine;
        private bool _canCast;

        public void Initialize(TowerModel towerModel, CastItem castItem)
        {
            if (_castSpawn == null)
            {
                Debug.LogError("Class: 'SpellCaster', Method: 'Initialize', Message: '_castSpawn == null'");
                return;
            }

            if (towerModel == null)
            {
                Debug.LogError("Class: 'SpellCaster', Method: 'Initialize', Message: 'towerModel == null'");
                return;
            }

            _towerModel = towerModel;
            
            if (castItem == null)
            {
                Debug.LogError("Class: 'SpellCaster', Method: 'Initialize', Message: 'castItem == null'");
                return;
            }

            _castItem = castItem;
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

        public void StartCasting() => _canCast = true;

        public void PauseCasting() => _canCast = false;

        private IEnumerator Casting()
        {
            while (_canCast == true && _target != null)
            {
                // TODO you need to use the object pool and add checks
                var missile = Instantiate(_castItem, _castSpawn.position, Quaternion.identity);
                
                CastItemModel castItemModel = new CastItemModel
                {
                    ElementalType = _towerModel.ElementalType,
                    Damage = _towerModel.Damage,
                };
                
                missile.Initialize(castItemModel, _target);
                yield return new WaitForSeconds(_towerModel.AttackSpeed);
            }
        }
    }
}
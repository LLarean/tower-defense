using System.Collections;
using DG.Tweening;
using Units;
using UnityEngine;
using Utilities;

namespace Towers
{
    public class CastItem : MonoBehaviour
    {
        private CastItemModel _castItemModel;
        private Tweener _tween;
        private Transform _target;
        private Vector3 _targetLastPosition;

        private bool _isInit = false;

        public CastItemModel CastItemModel => _castItemModel;

        public void Initialize(CastItemModel castItemModel)
        {
            if (castItemModel == null)
            {
                CustomLogger.LogError("castItemModel == null");
                return;
            }
            
            _castItemModel = castItemModel;
            _isInit = true;
        }

        public void SetTarget(Transform target)
        {
            if (target == null)
            {
                CustomLogger.LogError("target == null");
                return;
            }
            
            _target = target;
            _tween = transform.DOMove(target.position, .2f).SetAutoKill(false);
            _targetLastPosition = target.position;
            
            StartCoroutine(DelayDestroy());
        }

        private void Update()
        {
            // TODO redesign the goal pursuit system
            if (_isInit == false)
            {
                return;
            }

            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }
            
            if (_targetLastPosition != _target.position)
            {
                _tween.ChangeEndValue(_target.position, true).Restart();
                _targetLastPosition = _target.position;
            }
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }

        private void OnTriggerEnter(Collider collision)
        {
            var isAvailable = collision.TryGetComponent<Enemy>(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }
            
            enemy.TakeDamage(_castItemModel);
            _tween.Kill();
            Destroy(gameObject);
        }
    }
}
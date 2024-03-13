using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Builds
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private Type _type;
        
        private Tweener _tween;
        private Transform _target;
        private Vector3 _targetLastPosition;

        private bool _isInit = false;

        public void Init(Transform target)
        {
            _target = target;

            _tween = transform.DOMove(target.position, .2f).SetAutoKill(false);
            _targetLastPosition = target.position;
            
            StartCoroutine(Test());
            _isInit = true;
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            if (_isInit == false)
            {
                return;
            }
            
            if (_targetLastPosition != _target.position)
            {
                _tween.ChangeEndValue(_target.position, true).Restart();
                _targetLastPosition = _target.position;
            }
        }

        private IEnumerator Test()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnter");

            var isAvailable = collision.TryGetComponent<Enemy>(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }
            
            Destroy(gameObject);
        }
    }
}
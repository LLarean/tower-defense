﻿using System.Collections;
using DG.Tweening;
using Units;
using UnityEngine;

namespace Builds
{
    public class CastItem : MonoBehaviour
    {
        private CastItemModel _castItemModel;
        private Tweener _tween;
        private Transform _target;
        private Vector3 _targetLastPosition;

        private bool _isInit = false;

        public CastItemModel CastItemModel => _castItemModel;

        public void Initialize(CastItemModel castItemModel, Transform target)
        {
            if (castItemModel == null)
            {
                Debug.LogError("Class: 'CastItem', Method: 'Initialize', Message: 'castItemModel == null'");
                return;
            }

            if (target == null)
            {
                Debug.LogError("Class: 'CastItem', Method: 'Initialize', Message: 'target == null'");
                return;
            }
            
            _castItemModel = castItemModel;
            _target = target;

            _tween = transform.DOMove(target.position, .2f).SetAutoKill(false);
            _targetLastPosition = target.position;
            
            StartCoroutine(DelayDestroy());
            _isInit = true;
        }

        private void Update()
        {
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

        private void OnTriggerEnter(Collider collision)
        {
            var isAvailable = collision.TryGetComponent<Enemy>(out Enemy enemy);

            if (isAvailable == false)
            {
                return;
            }
            
            enemy.TakeDamage(CastItemModel);
            _tween.Kill();
            Destroy(gameObject);
        }
    }
}
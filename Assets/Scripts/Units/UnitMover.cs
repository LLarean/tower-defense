using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Units
{
    public class UnitMover : MonoBehaviour
    {
        private List<Transform> _wayPoints = new();
        private Tweener _tweener;
        private float _currentMoveSpeed;
        private int _currentPointIndex;
        
        public float CurrentMoveSpeed => _currentMoveSpeed;

        public void Initialize(float currentMoveSpeed, List<Transform> wayPoints)
        {
            _currentMoveSpeed = currentMoveSpeed;
            _wayPoints = wayPoints;
        }

        public void MoveToNextPoint()
        {
            _tweener?.Kill();
            StartTweener();
        }

        public void ChangeSpeed(float currentMoveSpeed)
        {
            _tweener?.Kill();
        
            _currentMoveSpeed = currentMoveSpeed;
            _currentPointIndex--;
        
            StartTweener();
        }

        private void StartTweener()
        {
            if (_currentPointIndex < 0 || _currentPointIndex >= _wayPoints.Count)
            {
                return;
            }
        
            float distance = Vector3.Distance(_wayPoints[_currentPointIndex].position, gameObject.transform.position);
            var duration = distance / _currentMoveSpeed;

            _tweener = gameObject.transform
                .DOMove(_wayPoints[_currentPointIndex].position, duration)
                .OnComplete(MoveToNextPoint);

            _currentPointIndex++;
        }
        
        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }
}
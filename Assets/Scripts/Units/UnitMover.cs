using DG.Tweening;
using UnityEngine;

namespace Units
{
    public class UnitMover : MonoBehaviour
    {
        private Vector3 _wayPoint;
        private Tweener _tweener;
        private float _currentMoveSpeed;
        
        public void Initialize(float currentMoveSpeed)
        {
            _currentMoveSpeed = currentMoveSpeed;
        }
        
        public void SetWayPoint(Vector3 nextWayPoint)
        {
            _wayPoint = nextWayPoint;
            MoveToNextPoint();
        }

        public void SetMoveSpeed(float currentMoveSpeed)
        {
            _tweener?.Kill();
            _currentMoveSpeed = currentMoveSpeed;
            StartTweener();
        }

        private void MoveToNextPoint()
        {
            _tweener?.Kill();
            StartTweener();
        }

        private void StartTweener()
        {
            float distance = Vector3.Distance(_wayPoint, gameObject.transform.position);
            var duration = distance / _currentMoveSpeed;

            _tweener = gameObject.transform.DOMove(_wayPoint, duration).SetEase(Ease.Linear);;
        }
        
        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }
}
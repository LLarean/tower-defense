using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] [Range(0, 200)] private float _moveSpeed = 20f;
    [SerializeField] private int _health;
    
    private List<Transform> _wayPoints;
    private Transform _finishPoint;
    private int _currentPointIndex = 0;


    public void Initialize(List<Transform> wayPoints, Transform finishPoint)
    {
        _wayPoints = wayPoints;
        _finishPoint = finishPoint;
        
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        if (_currentPointIndex < _wayPoints.Count)
        {
            float distance = Vector3.Distance(_wayPoints[_currentPointIndex].position,gameObject.transform.position);
            var duration = distance / _moveSpeed;
            
            gameObject.transform.DOMove(_wayPoints[_currentPointIndex].position, duration)
                .OnComplete(() =>
                {
                    _currentPointIndex++;
                    MoveToNextPoint();
                });
        }
        else
        {
            float distance = Vector3.Distance(_finishPoint.position,gameObject.transform.position);
            var duration = distance / _moveSpeed;
            
            gameObject.transform.DOMove(_finishPoint.position, duration);
        }
    }
}
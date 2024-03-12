using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<Transform> _wayPoints;
    private Transform _finishPoint;
    private int _currentPointIndex = 0;

    private float _speed = 1f;

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
            gameObject.transform.DOMove(_wayPoints[_currentPointIndex].position, _speed)
                .OnComplete(() =>
                {
                    _currentPointIndex++;
                    MoveToNextPoint();
                });
        }
        else
        {
            gameObject.transform.DOMove(_finishPoint.position, _speed);
        }
    }
}
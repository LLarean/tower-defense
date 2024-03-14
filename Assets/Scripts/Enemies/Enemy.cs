using System;
using System.Collections.Generic;
using Builds;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] [Range(0, 200)] private float _moveSpeed = 20f;
    [SerializeField] private int _maximumHealth;

    private int _currentHealth;
    private float _currentMoveSpeed;
    private bool _isEffect;
    
    private List<Transform> _wayPoints;
    private Transform _finishPoint;
    private int _currentPointIndex = 0;
    private Tweener _tweener;
    
    public event Action OnDestroyed;

    public void Initialize(List<Transform> wayPoints, Transform finishPoint)
    {
        _currentMoveSpeed = _moveSpeed;
        _currentHealth = _maximumHealth;
        
        _wayPoints = wayPoints;
        _finishPoint = finishPoint;
        
        MoveToNextPoint();
    }

    public void TakeDamage(Missile missile)
    {
        _maximumHealth -= missile.Damage;

        if (_maximumHealth <= 0)
        {
            _tweener.Kill();
            Destroy(gameObject);
        }
        else
        {
            TakeEffect(missile.Type);
        }
    }

    private void TakeEffect(Type type)
    {
        if (_isEffect == true)
        {
            return;
        }
        
        _isEffect = true;
        
        if (type == Type.Ice)
        {
            _currentMoveSpeed -= 15;
        }
    }

    private void MoveToNextPoint()
    {
        if (_currentPointIndex < _wayPoints.Count)
        {
            float distance = Vector3.Distance(_wayPoints[_currentPointIndex].position,gameObject.transform.position);
            var duration = distance / _currentMoveSpeed;
            
            _tweener = gameObject.transform.DOMove(_wayPoints[_currentPointIndex].position, duration)
                .OnComplete(() =>
                {
                    _currentPointIndex++;
                    MoveToNextPoint();
                });
        }
        else
        {
            float distance = Vector3.Distance(_finishPoint.position,gameObject.transform.position);
            var duration = distance / _currentMoveSpeed;
            
            _tweener = gameObject.transform.DOMove(_finishPoint.position, duration);
        }
    }
}
using System;
using System.Collections.Generic;
using Builds;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] [Range(0, 200)] private float _moveSpeed = 20f;
    [SerializeField] private int _maximumHealth;
    [SerializeField] private TMP_Text _statusBar;

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

        _statusBar.text = $"{_currentHealth.ToString()}/{_maximumHealth.ToString()}";
        
        _wayPoints = wayPoints;
        _finishPoint = finishPoint;
        
        MoveToNextPoint();
    }

    public void TakeDamage(CastItem castItem)
    {
        _maximumHealth -= castItem.Damage;

        if (_maximumHealth <= 0)
        {
            OnDestroyed?.Invoke();
            _tweener.Kill();
            Destroy(gameObject);
        }
        else
        {
            _statusBar.text = $"{_currentHealth.ToString()}/{_maximumHealth.ToString()}";
            TakeEffect(castItem.Type);
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
            _statusBar.text = $"{_statusBar.text} ({Type.Ice})";
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
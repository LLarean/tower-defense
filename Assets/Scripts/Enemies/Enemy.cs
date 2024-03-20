using System.Collections.Generic;
using Builds;
using DG.Tweening;
using Infrastructure;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] [Range(0, 200)] private float _moveSpeed = 20f;
    [SerializeField] private int _maximumHealth;
    [SerializeField] private TMP_Text _statusBar;
    [SerializeField] private ResistType _resistType;

    private int _currentHealth;
    private float _currentMoveSpeed;
    private bool _isEffect;
    
    private List<Transform> _wayPoints;
    private DestroyPoint _destroyPoint;
    private int _currentPointIndex = 0;
    private Tweener _tweener;

    public void Initialize(in PathModel pathModel)
    {
        _currentMoveSpeed = _moveSpeed;
        _currentHealth = _maximumHealth;

        _statusBar.text = $"{_currentHealth.ToString()}/{_maximumHealth.ToString()}";
        
        _wayPoints = pathModel.WayPoints;
        _destroyPoint = pathModel.DestroyPoint;
        
        MoveToNextPoint();
    }

    public void TakeDamage(CastItem castItem)
    {
        if (_resistType.ToString() == castItem.CastType.ToString())
        {
            var damage = castItem.Damage - GlobalParams.DamageReduction;
            _maximumHealth -= damage;
        }
        else
        {
            _maximumHealth -= castItem.Damage;
        }

        if (_maximumHealth <= 0)
        {
            EventBus.RaiseEvent<IEnemyHandler>(handler => handler.HandleDestroy());
            
            _tweener.Kill();
            Destroy(gameObject);
        }
        else
        {
            _statusBar.text = $"{_currentHealth.ToString()}/{_maximumHealth.ToString()}";
            TakeEffect(castItem.CastType);
        }
    }

    public void Finish()
    {
        EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleFinish());
    }

    private void TakeEffect(CastType castType)
    {
        if (_isEffect == true)
        {
            return;
        }
        
        
        if (castType == CastType.Ice && _resistType != ResistType.Ice)
        {
            _isEffect = true;
            _statusBar.text = $"{_statusBar.text} ({CastType.Ice})";
            _currentMoveSpeed -= GlobalParams.IceSlow;
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
            float distance = Vector3.Distance(_destroyPoint.gameObject.transform.position,gameObject.transform.position);
            var duration = distance / _currentMoveSpeed;
            
            _tweener = gameObject.transform.DOMove(_destroyPoint.gameObject.transform.position, duration);
        }
    }

    private void OnDestroy()
    {
        _tweener.Kill();
    }
}
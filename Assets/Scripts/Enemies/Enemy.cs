using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyModel _enemyModel;
    [SerializeField] private TMP_Text _statusBar;
    [SerializeField] private UnitMover _unitMover;

    private HealthLogic _healthLogic;
    
    private PathModel _pathModel;

    private int _currentPointIndex = 0;
    private Tweener _tweener;
    private Coroutine _coroutine;

    public void Initialize(in PathModel pathModel)
    {
        _pathModel = pathModel;
        
        _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
        _enemyModel.CurrentHealth = _enemyModel.MaximumHealth;
        
        DisplayHealth(_enemyModel.CurrentHealth, _enemyModel.CurrentHealth);
        
        _unitMover.Initialize(_enemyModel.CurrentMoveSpeed, _pathModel.WayPoints);
        _unitMover.MoveToNextPoint();

        _healthLogic = new HealthLogic(_enemyModel);
        
        _enemyModel.CurrentHealth.ValueChanged += DisplayHealth;
        _enemyModel.CurrentMoveSpeed.ValueChanged += ChangeMoveSpeed;
        _enemyModel.DebuffModels.ValueChanged += DisplayDebuffs;
    }
    
    public void TakeDamage(CastItemModel castItemModel)
    {
        _healthLogic.TakeDamage(castItemModel);
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(TickState());
    }

    private void DisplayHealth(int current, int previous)
    {
        _statusBar.text = $"{_enemyModel.CurrentHealth}/{_enemyModel.MaximumHealth.ToString()}";
    }
    
    private void ChangeMoveSpeed(float current, float previous)
    {
        _unitMover.ChangeSpeed(_enemyModel.CurrentMoveSpeed);
    }

    private void DisplayDebuffs(List<DebuffModel> current, List<DebuffModel> previous)
    {
        foreach (var debuffModel in _enemyModel.DebuffModels.Value)
        {
            _statusBar.text = $"{_statusBar.text} ({debuffModel.DebuffType})";
        }
    }

    private IEnumerator TickState()
    {
        while (_enemyModel.DebuffModels.Value.Count > 0)
        {
            yield return new WaitForSeconds(GlobalParams.TickTime);
            _healthLogic.UpdateDebuffsDuration();
        }
        
        _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
    }

    private void OnDestroy()
    {
        _tweener.Kill();
    }
}
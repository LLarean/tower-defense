using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Infrastructure;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyModel _enemyModel;
    [SerializeField] private TMP_Text _statusBar;

    private PathModel _pathModel;

    private int _currentPointIndex = 0;
    private Tweener _tweener;
    private Coroutine _coroutine;

    public void Initialize(in PathModel pathModel)
    {
        _pathModel = pathModel;
        
        _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
        _enemyModel.CurrentHealth = _enemyModel.MaximumHealth;
        
        DisplayHealth();
        MoveToNextPoint();
    }
    
    public void TakeDamage(CastItemModel castItemModel)
    {
        var damage = GetCalculatedDamage(castItemModel);
        _enemyModel.CurrentHealth -= damage;

        if (_enemyModel.CurrentHealth <= 0)
        {
            EventBus.RaiseEvent<IEnemyHandler>(handler => handler.HandleDestroy());

            // TODO add a gradual deletion logic
            Destroy(gameObject);
            return;
        }

        DisplayHealth();
        TakeEffect(castItemModel.CastType);
    }

    public void Finish()
    {
        EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleFinish());
    }

    private void TakeEffect(CastType castType)
    {
        if (_enemyModel.ResistType.ToString() == castType.ToString())
        {
            return;
        }
        
        SetDebuff(castType);

        if (_enemyModel.DebuffModels.Count == 0)
        {
            return;
        }
        
        ShowDebuffs();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(TickState());
    }

    private void SetDebuff(CastType castType)
    {
        UpdateDebuffTime(castType);
        UpdateDebuffModels(castType);
    }

    private IEnumerator TickState()
    {
        while (_enemyModel.DebuffModels.Count > 0)
        {
            yield return new WaitForSeconds(GlobalParams.TickTime);
            UpdateBuffsStatus();
        }
        
        _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
    }

    private void UpdateBuffsStatus()
    {
        foreach (var debuffModel in _enemyModel.DebuffModels)
        {
            debuffModel.Duration -= GlobalParams.TickTime;
        }
        
        List<DebuffModel> debuffModels = _enemyModel.DebuffModels.Where(item => item.Duration <= 0).ToList();
        _enemyModel.DebuffModels.RemoveAll(item => item.Duration <= 0);

        foreach (var debuffModel in debuffModels)
        {
            if (debuffModel.DebuffType == DebuffType.Burning)
            {
                _enemyModel.CurrentHealth -= GlobalParams.BurningDamage;
            }
            else if (debuffModel.DebuffType == DebuffType.Slow)
            {
                _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed - GlobalParams.IceSlow;
                ChangeSpeed();
            }
            else if (debuffModel.DebuffType == DebuffType.Frozen)
            {
                _enemyModel.CurrentMoveSpeed = 0;
                ChangeSpeed();
            }
        }
    }

    private void ChangeSpeed()
    {
        var index = _currentPointIndex - 1;
        
        if (index < _pathModel.WayPoints.Count)
        {
            float distance = Vector3.Distance(_pathModel.WayPoints[index].position,gameObject.transform.position);
            var duration = distance / _enemyModel.CurrentMoveSpeed;
            
            _tweener = gameObject.transform
                .DOMove(_pathModel.WayPoints[index].position, duration)
                .OnComplete(MoveToNextPoint);
        }
        else
        {
            float distance = Vector3.Distance(_pathModel.DestroyPoint.gameObject.transform.position,gameObject.transform.position);
            var duration = distance / _enemyModel.CurrentMoveSpeed;
            
            _tweener = gameObject.transform.DOMove(_pathModel.DestroyPoint.gameObject.transform.position, duration);
        }
    }

    private void MoveToNextPoint()
    {
        if (_currentPointIndex < _pathModel.WayPoints.Count)
        {
            float distance = Vector3.Distance(_pathModel.WayPoints[_currentPointIndex].position,gameObject.transform.position);
            var duration = distance / _enemyModel.CurrentMoveSpeed;
            
            _tweener = gameObject.transform.DOMove(_pathModel.WayPoints[_currentPointIndex].position, duration)
                .OnComplete(() =>
                {
                    _currentPointIndex++;
                    MoveToNextPoint();
                });
        }
        else
        {
            float distance = Vector3.Distance(_pathModel.DestroyPoint.gameObject.transform.position,gameObject.transform.position);
            var duration = distance / _enemyModel.CurrentMoveSpeed;
            
            _tweener = gameObject.transform.DOMove(_pathModel.DestroyPoint.gameObject.transform.position, duration);
        }
    }

    private void DisplayHealth()
    {
        _statusBar.text = $"{_enemyModel.CurrentHealth.ToString()}/{_enemyModel.MaximumHealth.ToString()}";
    }
    
    private void ShowDebuffs()
    {
        foreach (var debuffModel in _enemyModel.DebuffModels)
        {
            _statusBar.text = $"{_statusBar.text} ({debuffModel.DebuffType})";
        }
    }

    private int GetCalculatedDamage(CastItemModel castItemModel)
    {
        var damage = castItemModel.Damage;

        if (_enemyModel.ResistType.ToString() == castItemModel.CastType.ToString())
        {
            damage -= GlobalParams.DamageReduction;
        }

        return damage;
    }
    
    private void UpdateDebuffTime(CastType castType)
    {
        if (_enemyModel.DebuffModels.Count <= 0)
        {
            return;
        }
        
        foreach (var debuffModel in _enemyModel.DebuffModels)
        {
            debuffModel.Duration = castType switch
            {
                CastType.Fire when debuffModel.DebuffType == DebuffType.Burning => GlobalParams.DebuffTime,
                CastType.Water when debuffModel.DebuffType == DebuffType.Wet => GlobalParams.DebuffTime,
                CastType.Ice when debuffModel.DebuffType == DebuffType.Slow => GlobalParams.DebuffTime,
                _ => debuffModel.Duration
            };
        }
    }
    
    // TODO it needs to be recycled. should I put it in the interface?
     private void UpdateDebuffModels(CastType castType)
    {
        if (_enemyModel.DebuffModels.Count <= 0)
        {
            DebuffModel debuffModel = new DebuffModel
            {
                Duration = GlobalParams.DebuffTime,
                DebuffType = castType switch
                {
                    CastType.Fire => DebuffType.Burning,
                    CastType.Water => DebuffType.Wet,
                    CastType.Ice => DebuffType.Slow,
                    _ => DebuffType.Burning
                }
            };

            _enemyModel.DebuffModels.Add(debuffModel);
            return;
        }
        
        foreach (var debuffModel in _enemyModel.DebuffModels)
        {
            if (debuffModel.DebuffType == DebuffType.Burning && castType == CastType.Water)
            {
                _enemyModel.DebuffModels.Remove(debuffModel);
                break;
            }

            if (debuffModel.DebuffType == DebuffType.Burning && castType == CastType.Ice)
            {
                _enemyModel.DebuffModels.Remove(debuffModel);
                break;
            }

            if (debuffModel.DebuffType == DebuffType.Burning && castType == CastType.Ice)
            {
                _enemyModel.DebuffModels.Remove(debuffModel);
                break;
            }

            if (debuffModel.DebuffType == DebuffType.Wet && castType == CastType.Fire)
            {
                _enemyModel.DebuffModels.Remove(debuffModel);
                break;
            }

            if (debuffModel.DebuffType == DebuffType.Wet && castType == CastType.Ice)
            {
                DebuffModel newDebuffModel = new DebuffModel
                {
                    DebuffType = DebuffType.Frozen,
                    Duration = GlobalParams.DebuffTime,
                };

                _enemyModel.DebuffModels.Add(newDebuffModel);
                _enemyModel.DebuffModels.Remove(debuffModel);
                break;
            }

            if (debuffModel.DebuffType == DebuffType.Slow && castType == CastType.Fire)
            {
                _enemyModel.DebuffModels.Remove(debuffModel);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        _tweener.Kill();
    }
}
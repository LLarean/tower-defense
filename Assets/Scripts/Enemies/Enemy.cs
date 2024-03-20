using System.Collections;
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
        
        ShowHealth();
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

        ShowHealth();
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

        if (_enemyModel.Debuffs != Debuff.None)
        {
            _statusBar.text = $"{_statusBar.text} ({_enemyModel.Debuffs})";
        }
        else
        {
            ShowHealth();
        }
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(DebuffTimer(GlobalParams.DebuffTime));
    }

    private void SetDebuff(CastType castType)
    {
        _enemyModel.Debuffs = castType switch
        {
            CastType.Fire when _enemyModel.Debuffs == Debuff.None => Debuff.Burning,
            CastType.Fire when _enemyModel.Debuffs == Debuff.Burning => Debuff.Burning,
            CastType.Fire when _enemyModel.Debuffs == Debuff.Wet => Debuff.None,
            CastType.Fire when _enemyModel.Debuffs == Debuff.Slow => Debuff.None,
            CastType.Fire when _enemyModel.Debuffs == Debuff.Frozen => Debuff.Wet,
            
            CastType.Water when _enemyModel.Debuffs == Debuff.None => Debuff.Wet,
            CastType.Water when _enemyModel.Debuffs == Debuff.Burning => Debuff.None,
            CastType.Water when _enemyModel.Debuffs == Debuff.Wet => Debuff.Wet,
            CastType.Water when _enemyModel.Debuffs == Debuff.Slow => Debuff.Slow,
            CastType.Water when _enemyModel.Debuffs == Debuff.Frozen => Debuff.Frozen,
            
            CastType.Ice when _enemyModel.Debuffs == Debuff.None => Debuff.Slow,
            CastType.Ice when _enemyModel.Debuffs == Debuff.Burning => Debuff.None,
            CastType.Ice when _enemyModel.Debuffs == Debuff.Wet => Debuff.Frozen,
            CastType.Ice when _enemyModel.Debuffs == Debuff.Slow => Debuff.Slow,
            CastType.Ice when _enemyModel.Debuffs == Debuff.Frozen => Debuff.Frozen,
            _ => _enemyModel.Debuffs
        };
    }

    private IEnumerator DebuffTimer(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        _enemyModel.Debuffs = Debuff.None;
        _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
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

    private void ShowHealth()
    {
        _statusBar.text = $"{_enemyModel.CurrentHealth.ToString()}/{_enemyModel.MaximumHealth.ToString()}";
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

    private void OnDestroy()
    {
        _tweener.Kill();
    }
}
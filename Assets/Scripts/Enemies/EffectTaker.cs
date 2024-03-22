using System.Collections.Generic;
using System.Linq;

public class EffectTaker
{
    private EnemyModel _enemyModel;

    public void Initialize(EnemyModel enemyModel)
    {
        _enemyModel = enemyModel;
    }
    
    public void UpdateBuffsStatus()
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
                // _unitMover.ChangeSpeed(_enemyModel.CurrentMoveSpeed);
            }
            else if (debuffModel.DebuffType == DebuffType.Frozen)
            {
                _enemyModel.CurrentMoveSpeed = 0;
                // _unitMover.ChangeSpeed(_enemyModel.CurrentMoveSpeed);
            }
        }
    }

    private void DisplayHealth()
    {
        // _statusBar.text = $"{_enemyModel.CurrentHealth.ToString()}/{_enemyModel.MaximumHealth.ToString()}";
    }
    
    private void ShowDebuffs()
    {
        foreach (var debuffModel in _enemyModel.DebuffModels)
        {
            // _statusBar.text = $"{_statusBar.text} ({debuffModel.DebuffType})";
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
}
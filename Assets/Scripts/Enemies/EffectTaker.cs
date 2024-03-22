// TODO it needs to be renamed
public class HealthLogic
{
    private EnemyModel _enemyModel;
    
    public HealthLogic(EnemyModel enemyModel)
    {
        _enemyModel = enemyModel;
    }
    
    public void TakeDamage(CastItemModel castItemModel)
    {
        var damage = GetCalculatedDamage(castItemModel);
        _enemyModel.CurrentHealth.Value -= damage;

        if (_enemyModel.CurrentHealth <= 0)
        {
            return;
        }

        UpdateDebuffModels(castItemModel.CastType);
    }

    public void UpdateDebuffsDuration()
    {
        foreach (var debuffModel in _enemyModel.DebuffModels.Value)
        {
            debuffModel.Duration -= GlobalParams.TickTime;
        }
        
        // List<DebuffModel> debuffModels = _enemyModel.DebuffModels.Value.Where(item => item.Duration <= 0).ToList();
        _enemyModel.DebuffModels.Value.RemoveAll(item => item.Duration <= 0);
        TakeEffect();
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

    private void UpdateDebuffModels(CastType castType)
    {
        if (_enemyModel.DebuffModels.Value.Count <= 0)
        {
            AddNewDebuff(castType);
        }
        else
        {
            UpdateDebuffModel(castType);
        }
    }

    private void AddNewDebuff(CastType castType)
    {
        DebuffModel debuffModel = new DebuffModel
        {
            Duration = GlobalParams.DebuffDuration,
            DebuffType = castType switch
            {
                CastType.Fire => DebuffType.Burning,
                CastType.Water => DebuffType.Wet,
                CastType.Ice => DebuffType.Slow,
                _ => DebuffType.Burning
            }
        };

        _enemyModel.DebuffModels.Value.Add(debuffModel);
    }

    private void UpdateDebuffModel(CastType castType)
    {
        foreach (var debuffModel in _enemyModel.DebuffModels.Value)
        {
            var isRemoved = TryRemoveDebuff(debuffModel, castType);

            if (isRemoved == true)
            {
                break;
            }

            var isReplaced = TryReplaceEffect(debuffModel, castType);
            
            if (isReplaced == true)
            {
                break;
            }
            
            var isUpdate = TryUpdateDebuffDuration(debuffModel, castType);
            
            if (isUpdate == true)
            {
                break;
            }
        }
    }

    private bool TryRemoveDebuff(DebuffModel debuffModel, CastType castType)
    {
        bool isSuccess = false;

        if (debuffModel.DebuffType == DebuffType.Burning && castType == CastType.Ice)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);
            isSuccess = true;
        }
        else if (debuffModel.DebuffType == DebuffType.Burning && castType == CastType.Water)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);
            isSuccess = true;
        }
        else if (debuffModel.DebuffType == DebuffType.Wet && castType == CastType.Fire)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);
            isSuccess = true;
        }
        else if (debuffModel.DebuffType == DebuffType.Slow && castType == CastType.Fire)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);
            isSuccess = true;
        }
        else if (debuffModel.DebuffType == DebuffType.Frozen && castType == CastType.Fire)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);
            isSuccess = true;
        }
        
        return isSuccess;
    }
    
    private bool TryReplaceEffect(DebuffModel debuffModel, CastType castType)
    {
        bool isSuccess = false;
        
        if (debuffModel.DebuffType == DebuffType.Wet && castType == CastType.Ice)
        {
            AddNewDebuff(castType);
            _enemyModel.DebuffModels.Value.Remove(debuffModel);
            isSuccess = true;
        }

        return isSuccess;
    }
    
    private bool TryUpdateDebuffDuration(DebuffModel debuffModel, CastType castType)
    {
        bool isSuccess = false;

        if (debuffModel.DebuffType == DebuffType.Burning && castType == CastType.Fire)
        {
            debuffModel.Duration = GlobalParams.DebuffDuration;
            isSuccess = true;
        }
        else if (debuffModel.DebuffType == DebuffType.Wet && castType == CastType.Water)
        {
            debuffModel.Duration = GlobalParams.DebuffDuration;
            isSuccess = true;
        }
        else if (debuffModel.DebuffType == DebuffType.Slow && castType == CastType.Ice)
        {
            debuffModel.Duration = GlobalParams.DebuffDuration;
            isSuccess = true;
        }

        return isSuccess;
    }
    
    private void TakeEffect()
    {
        foreach (var debuffModel in _enemyModel.DebuffModels.Value)
        {
            if (debuffModel.DebuffType == DebuffType.Burning)
            {
                _enemyModel.CurrentHealth -= GlobalParams.BurningDamage;
            }
            else if (debuffModel.DebuffType == DebuffType.Slow)
            {
                _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed - GlobalParams.IceSlow;
            }
            else if (debuffModel.DebuffType == DebuffType.Frozen)
            {
                _enemyModel.CurrentMoveSpeed = 0;
            }
        }
    }
}
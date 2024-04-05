using System.Collections.Generic;
using System.Linq;
using Builds;
using Units;

public class UnitEffects
{
    private List<EffectModel> _activeEffects = new List<EffectModel>();
    private List<DebuffModel> _debuffModels = new List<DebuffModel>();

    public List<EffectModel> ActiveEffects => _activeEffects;
    public List<DebuffModel> DebuffModels => _debuffModels;

    public void TakeEffect(ElementalType elementalType)
    {
        AddNewEffect(elementalType);
        UpdateDebuffModels();
    }

    public void UpdateTime()
    {
        foreach (var debuffModel in _debuffModels)
        {
            debuffModel.Duration -= GlobalParams.TickTime;
        }
    }

    private void AddNewEffect(ElementalType elementalType)
    {
        var effectModel = new EffectModel(elementalType, GlobalParams.DebuffDuration);
        var isRemoved = TryRemoveConflictingEffects(effectModel);
        var isUpdated = TryUpdateEffects(effectModel);

        if (isRemoved == false && isUpdated == false)
        {
            _activeEffects.Add(effectModel);
        }
    }

    private bool TryRemoveConflictingEffects(EffectModel effectModel)
    {
        var isRemoved = false;
        List<EffectModel> conflictingEffects = GetConflictingEffects(effectModel);

        if (conflictingEffects.Count == 0)
        {
            return isRemoved;
        }

        _activeEffects = _activeEffects.Except(conflictingEffects).ToList();
        isRemoved = true;
        return isRemoved;
    }
    
    private List<EffectModel> GetConflictingEffects(EffectModel effectModel)
    {
        var removedEffects = new List<EffectModel>();
        
        foreach (var activeEffect in _activeEffects)
        {
            switch (activeEffect.ElementalType)
            {
                case ElementalType.Water when effectModel.ElementalType == ElementalType.Fire:
                case ElementalType.Ice when effectModel.ElementalType == ElementalType.Fire:
                    removedEffects.Add(activeEffect);
                    break;
                case ElementalType.Fire when effectModel.ElementalType == ElementalType.Water:
                case ElementalType.Fire when effectModel.ElementalType == ElementalType.Ice:
                    removedEffects.Add(activeEffect);
                    break;
            }
        }

        return removedEffects;
    }
    
    private bool TryUpdateEffects(EffectModel effectModel)
    {
        var isUpdated = false;

        foreach (var activeEffect in _activeEffects.Where(activeEffect => activeEffect.ElementalType == effectModel.ElementalType))
        {
            activeEffect.Duration = GlobalParams.EffectDuration;
            isUpdated = true;
            return isUpdated;
        }

        return isUpdated;
    }
    
    private void UpdateDebuffModels()
    {
        if (_activeEffects.Count == 0)
        {
            _debuffModels.Clear();
            return;
        }
        
        bool canAddDebuff;
        
        DebuffModel debuffModel = new DebuffModel()
        {
            Duration = GlobalParams.DebuffDuration,
        };
        
        canAddDebuff = TryGetDebuffBasedTwoEffects(in debuffModel);

        if (canAddDebuff == false)
        {
            canAddDebuff = TryGetDebuffBasedEffect(in debuffModel);
        }
        
        if (canAddDebuff == true)
        {
            RemoveConflictingDebuffs(debuffModel);
            _debuffModels.Add(debuffModel);
        }
    }

    private bool TryGetDebuffBasedTwoEffects(in DebuffModel debuffModel)
    {
        var isSuccess = false;

        foreach (var activeEffect in _activeEffects)
        {
            foreach (var effect in _activeEffects)
            {
                if (activeEffect.ElementalType == ElementalType.Water && effect.ElementalType == ElementalType.Ice)
                {
                    debuffModel.DebuffType = DebuffType.Frozen;
                    isSuccess = true;
                    return isSuccess;
                }
            
                if (activeEffect.ElementalType == ElementalType.Ice && effect.ElementalType == ElementalType.Water)
                {
                    debuffModel.DebuffType = DebuffType.Frozen;
                    isSuccess = true;
                    return isSuccess;
                }
            }
        }
        
        return isSuccess;
    }
    
    private bool TryGetDebuffBasedEffect(in DebuffModel debuffModel)
    {
        var isSuccess = false;
        
        foreach (var activeEffect in _activeEffects)
        {
            if (activeEffect.ElementalType == ElementalType.Fire)
            {
                debuffModel.DebuffType = DebuffType.Burning;
                isSuccess = true;
                return isSuccess;
            }
            
            if (activeEffect.ElementalType == ElementalType.Poison)
            {
                debuffModel.DebuffType = DebuffType.Intoxication;
                isSuccess = true;
                return isSuccess;
            }
            
            if (activeEffect.ElementalType == ElementalType.Water)
            {
                debuffModel.DebuffType = DebuffType.Wet;
                isSuccess = true;
                return isSuccess;
            }
            
            if (activeEffect.ElementalType == ElementalType.Ice)
            {
                debuffModel.DebuffType = DebuffType.Slow;
                isSuccess = true;
                return isSuccess;
            }
        }

        return isSuccess;
    }
    
    private void RemoveConflictingDebuffs(DebuffModel debuffModel)
    {
        var conflictingDebuffs = new List<DebuffModel>();
        
        foreach (var model in _debuffModels)
        {
            switch (model.DebuffType)
            {
                case DebuffType.Wet when debuffModel.DebuffType == DebuffType.Frozen:
                case DebuffType.Slow when debuffModel.DebuffType == DebuffType.Frozen:
                    conflictingDebuffs.Add(model);
                    break;
            }
        }
        
        _debuffModels = _debuffModels.Except(conflictingDebuffs).ToList();

    }
}
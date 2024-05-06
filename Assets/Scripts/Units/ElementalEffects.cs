using System.Collections.Generic;
using System.Linq;
using Towers;

namespace Units
{
    public class ElementalEffects
    {
        private readonly ElementalType _elementalResist;
        
        private List<EffectModel> _currentEffects = new List<EffectModel>();
        private List<DebuffModel> _activeDebuffs = new List<DebuffModel>();
        private List<EffectModel> _removedEffects = new List<EffectModel>();

        public List<EffectModel> CurrentEffects => _currentEffects;
        public List<DebuffModel> ActiveDebuffs => _activeDebuffs;

        public ElementalEffects(ElementalType elementalResist)
        {
            _elementalResist = elementalResist;
        }
        
        public void TakeEffect(ElementalType elementalType)
        {
            AddNewEffect(elementalType);
            UpdateActiveDebuffs();
        }

        public void UpdateDuration(float tickTime)
        {
            foreach (var activeEffect in _currentEffects)
            {
                activeEffect.Duration -= tickTime;
            }

            RemoveEffectsWithExpiredTime();
            UpdateActiveDebuffs();
        }

        public bool TryGetUpdatedDebuffModels(out List<DebuffModel> debuffModels)
        {
            var isSuccess = false;
            debuffModels = new List<DebuffModel>();
            
            if (_removedEffects.Count > 0)
            {
                _removedEffects.Clear();
                debuffModels = _activeDebuffs;
                isSuccess = true;
            }

            return isSuccess;
        }

        private void AddNewEffect(ElementalType elementalType)
        {
            if (_elementalResist == elementalType)
            {
                return;
            }
            
            var effectModel = new EffectModel(elementalType, GlobalParams.DebuffDuration);
            var isUpdated = TryUpdateEffectDuration(effectModel);
            var isRemoved = TryRemoveConflictingEffects(effectModel);

            if (isUpdated == false && isRemoved == false)
            {
                _currentEffects.Add(effectModel);
            }
        }
        
        private bool TryUpdateEffectDuration(EffectModel effectModel)
        {
            var isUpdated = false;

            foreach (var activeEffect in _currentEffects.Where(activeEffect => activeEffect.ElementalType == effectModel.ElementalType))
            {
                activeEffect.Duration = GlobalParams.EffectDuration;
                isUpdated = true;
                return isUpdated;
            }

            return isUpdated;
        }

        private bool TryRemoveConflictingEffects(EffectModel effectModel)
        {
            var isRemoved = false;
            List<EffectModel> conflictingEffects = GetConflictingEffects(effectModel);

            if (conflictingEffects.Count != 0)
            {
                _currentEffects = _currentEffects.Except(conflictingEffects).ToList();
                isRemoved = true;
                return isRemoved;
            }
            
            return isRemoved;
        }
    
        private List<EffectModel> GetConflictingEffects(EffectModel effectModel)
        {
            var removedEffects = new List<EffectModel>();
        
            foreach (var activeEffect in _currentEffects)
            {
                switch (activeEffect.ElementalType)
                {
                    case ElementalType.Water when effectModel.ElementalType == ElementalType.Fire:
                    case ElementalType.Ice when effectModel.ElementalType == ElementalType.Fire:
                        removedEffects.Add(activeEffect);
                        break;
                    case ElementalType.Fire when effectModel.ElementalType == ElementalType.Water:
                    case ElementalType.Poison when effectModel.ElementalType == ElementalType.Water:
                    case ElementalType.Fire when effectModel.ElementalType == ElementalType.Ice:
                        removedEffects.Add(activeEffect);
                        break;
                    case ElementalType.Water when effectModel.ElementalType == ElementalType.Poison:
                        removedEffects.Add(activeEffect);
                        break;
                }
            }

            return removedEffects;
        }
        
        private void UpdateActiveDebuffs()
        {
            if (_currentEffects.Count == 0)
            {
                _activeDebuffs.Clear();
                return;
            }

            var debuffModels = new List<DebuffModel>();
            var isSuccess = TryGetDebuffsBasedTwoEffects(out debuffModels);

            if (isSuccess != true)
            {
                isSuccess = TryGetDebuffsBasedEffect(out debuffModels);
            }
            
            if (isSuccess == true)
            {
                _activeDebuffs = debuffModels;
            }
        }

        private bool TryGetDebuffsBasedTwoEffects(out List<DebuffModel> debuffModels)
        {
            var isSuccess = false;
            var debuffModel = new DebuffModel();
            debuffModels = new List<DebuffModel>();
        
            foreach (var activeEffect in _currentEffects)
            {
                foreach (var effect in _currentEffects)
                {
                    if (activeEffect.ElementalType == ElementalType.Water && effect.ElementalType == ElementalType.Ice)
                    {
                        debuffModel = GetDebuffModel(DebuffType.Frozen, activeEffect.Duration);
                        debuffModels.Add(debuffModel);
                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }

        private bool TryGetDebuffsBasedEffect(out List<DebuffModel> debuffModels)
        {
            var isSuccess = false;
            var debuffModel = new DebuffModel();
            debuffModels = new List<DebuffModel>();
            
            foreach (var activeEffect in _currentEffects)
            {
                if (activeEffect.ElementalType == ElementalType.Fire)
                {
                    debuffModel = GetDebuffModel(DebuffType.Burning, activeEffect.Duration);
                }
                else if (activeEffect.ElementalType == ElementalType.Poison)
                {
                    debuffModel = GetDebuffModel(DebuffType.Intoxication, activeEffect.Duration);
                }
                else if (activeEffect.ElementalType == ElementalType.Water)
                {
                    debuffModel = GetDebuffModel(DebuffType.Wet, activeEffect.Duration);
                }
                else if (activeEffect.ElementalType == ElementalType.Ice)
                {
                    debuffModel = GetDebuffModel(DebuffType.Slow, activeEffect.Duration);
                }
            
                debuffModels.Add(debuffModel);
                isSuccess = true;
            }

            return isSuccess;
        }

        private DebuffModel GetDebuffModel(DebuffType debuffType, float duration)
        {
            var debuffModel = new DebuffModel()
            {
                DebuffType = debuffType,
                Duration = duration,
            };

            return debuffModel;
        }

        private void RemoveEffectsWithExpiredTime()
        {
            _removedEffects = _currentEffects.Where(item => item.Duration <= 0).ToList();
            _currentEffects = _currentEffects.Except(_removedEffects).ToList();
        }
    }
}
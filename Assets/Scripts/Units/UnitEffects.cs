using System.Collections.Generic;
using System.Linq;
using Builds;

namespace Units
{
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
        
            var debuffModels = new List<DebuffModel>();
            debuffModels = GetDebuffsBasedTwoEffects(debuffModels);

            if (debuffModels.Count == 0)
            {
                debuffModels = GetDebuffsBasedEffect(debuffModels);
            }
        
            if (debuffModels.Count != 0)
            {
                _debuffModels = debuffModels;
            }
        }

        private List<DebuffModel> GetDebuffsBasedTwoEffects(List<DebuffModel> debuffModels)
        {
            var debuffModel = new DebuffModel();
        
            foreach (var activeEffect in _activeEffects)
            {
                foreach (var effect in _activeEffects)
                {
                    if (activeEffect.ElementalType == ElementalType.Water && effect.ElementalType == ElementalType.Ice)
                    {
                        debuffModel = GetDebuffModel(DebuffType.Frozen);
                        debuffModels.Add(debuffModel);
                    }
            
                    if (activeEffect.ElementalType == ElementalType.Ice && effect.ElementalType == ElementalType.Water)
                    {
                        debuffModel = GetDebuffModel(DebuffType.Frozen);
                        debuffModels.Add(debuffModel);
                    }
                }
            }

            return debuffModels;
        }

        private DebuffModel GetDebuffModel(DebuffType debuffType)
        {
            var debuffModel = new DebuffModel()
            {
                Duration = GlobalParams.DebuffDuration,
                DebuffType = debuffType,
            };

            return debuffModel;
        }

        private List<DebuffModel> GetDebuffsBasedEffect(List<DebuffModel> debuffModels)
        {
            var debuffModel = new DebuffModel();
            
            foreach (var activeEffect in _activeEffects)
            {
                if (activeEffect.ElementalType == ElementalType.Fire)
                {
                    debuffModel = GetDebuffModel(DebuffType.Burning);
                }
                else if (activeEffect.ElementalType == ElementalType.Poison)
                {
                    debuffModel = GetDebuffModel(DebuffType.Intoxication);
                }
                else if (activeEffect.ElementalType == ElementalType.Water)
                {
                    debuffModel = GetDebuffModel(DebuffType.Wet);
                }
                else if (activeEffect.ElementalType == ElementalType.Ice)
                {
                    debuffModel = GetDebuffModel(DebuffType.Slow);
                }
            
                debuffModels.Add(debuffModel);
            }

            return debuffModels;
        }
    }
}
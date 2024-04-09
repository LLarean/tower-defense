using System.Collections.Generic;
using System.Linq;
using Builds;

namespace Units
{
    public class UnitHealth
    {
        private readonly EnemyModel _enemyModel;
        private readonly ElementalEffects _elementalEffects;

        public UnitHealth(EnemyModel enemyModel)
        {
            _enemyModel = enemyModel;
            _elementalEffects = new ElementalEffects();
        }
    
        public void TakeDamage(CastItemModel castItemModel)
        {
            var damage = GetCalculatedDamage(castItemModel);
            _enemyModel.CurrentHealth.Value -= damage;

            if (_enemyModel.CurrentHealth <= 0)
            {
                return;
            }

            UpdateDebuffModels(castItemModel.ElementalType);
        }

        public void UpdateDebuffsDuration()
        {
            _elementalEffects.UpdateDuration(GlobalParams.TickTime);

            var debuffModels = _enemyModel.DebuffModels.Value;
            // var isSuccess = _unitEffects.TryGetDebuffsExpiredTime(out debuffModels);

            // if (isSuccess == true)
            // {
                // _enemyModel.DebuffModels.Value = debuffModels;
            // }
            
            List<DebuffModel> removedDebuffModels = _elementalEffects.ActiveDebuffs.Where(item => item.Duration <= 0).ToList();

            // List<DebuffModel> removedDebuffModels = _enemyModel.DebuffModels.Value.Where(item => item.Duration <= 0).ToList();
            _enemyModel.DebuffModels.Value.RemoveAll(item => item.Duration <= 0);
            
            if (removedDebuffModels.Count != 0)
            {
                RemoveEffects(removedDebuffModels);
                
                // var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value);
                // _enemyModel.DebuffModels.Value = debuffModels;
            }
            
            TakeEffect();
        }

        private int GetCalculatedDamage(CastItemModel castItemModel)
        {
            var damage = castItemModel.Damage;

            if (_enemyModel.ElementalResist == castItemModel.ElementalType)
            {
                damage -= GlobalParams.DamageReduction;
            }
            
            return damage;
        }

        private void UpdateDebuffModels(ElementalType elementalType)
        {
            if (_enemyModel.ElementalResist == elementalType)
            {
                return;
            }
            
            _elementalEffects.TakeEffect(elementalType);
        }
        
        private void RemoveDebuff(DebuffModel debuffModel)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);

            var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value);
            _enemyModel.DebuffModels.Value = debuffModels;
        }

        private void TakeEffect()
        {
            foreach (var debuffModel in _enemyModel.DebuffModels.Value)
            {
                if (debuffModel.DebuffType == DebuffType.Burning)
                {
                    _enemyModel.CurrentHealth.Value -= GlobalParams.BurningDamage;
                }
                else if (debuffModel.DebuffType == DebuffType.Intoxication)
                {
                    _enemyModel.CurrentHealth.Value -= GlobalParams.IntoxicationDamage;
                }
                else if (debuffModel.DebuffType == DebuffType.Slow)
                {
                    _enemyModel.CurrentMoveSpeed.Value = _enemyModel.MoveSpeed - GlobalParams.IceSlow;
                }
                else if (debuffModel.DebuffType == DebuffType.Frozen)
                {
                    _enemyModel.CurrentMoveSpeed.Value = 0;
                }
            }
        }
        
        private void RemoveEffects(List<DebuffModel> debuffModels)
        {
            foreach (var debuffModel in debuffModels)
            {
                if (debuffModel.DebuffType == DebuffType.Slow)
                {
                    _enemyModel.CurrentMoveSpeed.Value = _enemyModel.MoveSpeed;
                }
                else if (debuffModel.DebuffType == DebuffType.Frozen)
                {
                    _enemyModel.CurrentMoveSpeed.Value = _enemyModel.MoveSpeed;
                }
            }
        }
    }
}
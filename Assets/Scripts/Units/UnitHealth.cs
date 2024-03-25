using System.Collections.Generic;
using System.Linq;
using Builds;

namespace Units
{
    public class UnitHealth
    {
        private readonly EnemyModel _enemyModel;
    
        public UnitHealth(EnemyModel enemyModel)
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
        
            List<DebuffModel> removedDebuffModels = _enemyModel.DebuffModels.Value.Where(item => item.Duration <= 0).ToList();
            _enemyModel.DebuffModels.Value.RemoveAll(item => item.Duration <= 0);
            
            if (removedDebuffModels.Count != 0)
            {
                RemoveEffects(removedDebuffModels);
                
                var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value);
                _enemyModel.DebuffModels.Value = debuffModels;
            }
            
            TakeEffect();
        }

        private int GetCalculatedDamage(CastItemModel castItemModel)
        {
            var damage = castItemModel.Damage;

            switch (_enemyModel.ResistType)
            {
                case ResistType.Fire when castItemModel.CastType == CastType.Fire:
                case ResistType.Ice when castItemModel.CastType == CastType.Ice:
                    damage -= GlobalParams.DamageReduction;
                    break;
            }

            return damage;
        }

        private void UpdateDebuffModels(CastType castType)
        {
            switch (_enemyModel.ResistType)
            {
                case ResistType.Fire when castType == CastType.Fire:
                case ResistType.Ice when castType == CastType.Ice:
                    return;
            }
                
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

            var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value)
            {
                debuffModel
            };
            
            _enemyModel.DebuffModels.Value = debuffModels;
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
                RemoveDebuff(debuffModel);
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

        private void RemoveDebuff(DebuffModel debuffModel)
        {
            _enemyModel.DebuffModels.Value.Remove(debuffModel);

            var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value);
            _enemyModel.DebuffModels.Value = debuffModels;
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
                    _enemyModel.CurrentHealth.Value -= GlobalParams.BurningDamage;
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
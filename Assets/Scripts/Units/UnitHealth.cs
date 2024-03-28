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

            UpdateDebuffModels(castItemModel.ElementalType);
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
            
            if (_enemyModel.DebuffModels.Value.Count <= 0)
            {
                AddNewDebuff(elementalType);
            }
            else
            {
                UpdateDebuffModel(elementalType);
            }
        }

        private void AddNewDebuff(ElementalType elementalType)
        {
            DebuffModel debuffModel = new DebuffModel
            {
                Duration = GlobalParams.DebuffDuration,
                DebuffType = elementalType switch
                {
                    ElementalType.Fire => DebuffType.Burning,
                    ElementalType.Poison => DebuffType.Intoxication,
                    ElementalType.Water => DebuffType.Wet,
                    ElementalType.Ice => DebuffType.Slow,
                    _ => DebuffType.Burning
                }
            };

            var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value)
            {
                debuffModel
            };
            
            _enemyModel.DebuffModels.Value = debuffModels;
        }

        private void UpdateDebuffModel(ElementalType elementalType)
        {
            foreach (var debuffModel in _enemyModel.DebuffModels.Value)
            {
                var isRemoved = TryRemoveDebuff(debuffModel, elementalType);

                if (isRemoved == true)
                {
                    break;
                }

                var isReplaced = TryReplaceEffect(debuffModel, elementalType);
            
                if (isReplaced == true)
                {
                    break;
                }
            
                var isUpdate = TryUpdateDebuffDuration(debuffModel, elementalType);
            
                if (isUpdate == true)
                {
                    break;
                }
            }
        }

        private bool TryRemoveDebuff(DebuffModel debuffModel, ElementalType elementalType)
        {
            bool isSuccess = false;

            if (debuffModel.DebuffType == DebuffType.Burning && elementalType == ElementalType.Ice)
            {
                RemoveDebuff(debuffModel);
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Burning && elementalType == ElementalType.Water)
            {
                _enemyModel.DebuffModels.Value.Remove(debuffModel);
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Intoxication && elementalType == ElementalType.Water)
            {
                _enemyModel.DebuffModels.Value.Remove(debuffModel);
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Wet && elementalType == ElementalType.Fire)
            {
                _enemyModel.DebuffModels.Value.Remove(debuffModel);
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Slow && elementalType == ElementalType.Fire)
            {
                _enemyModel.DebuffModels.Value.Remove(debuffModel);
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Frozen && elementalType == ElementalType.Fire)
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

        private bool TryReplaceEffect(DebuffModel debuffModel, ElementalType elementalType)
        {
            bool isSuccess = false;
        
            if (debuffModel.DebuffType == DebuffType.Wet && elementalType == ElementalType.Ice)
            {
                _enemyModel.DebuffModels.Value.Remove(debuffModel);
                
                DebuffModel newDebuff = new DebuffModel
                {
                    DebuffType = DebuffType.Frozen,
                    Duration = GlobalParams.DebuffDuration,
                };
                
                // _enemyModel.DebuffModels.Value.Add(newDebuff);
                
                var debuffModels = new List<DebuffModel>(_enemyModel.DebuffModels.Value)
                {
                    newDebuff
                };
            
                _enemyModel.DebuffModels.Value = debuffModels;
                
                isSuccess = true;
            }

            return isSuccess;
        }
    
        private bool TryUpdateDebuffDuration(DebuffModel debuffModel, ElementalType elementalType)
        {
            bool isSuccess = false;

            if (debuffModel.DebuffType == DebuffType.Burning && elementalType == ElementalType.Fire)
            {
                debuffModel.Duration = GlobalParams.DebuffDuration;
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Wet && elementalType == ElementalType.Water)
            {
                debuffModel.Duration = GlobalParams.DebuffDuration;
                isSuccess = true;
            }
            else if (debuffModel.DebuffType == DebuffType.Slow && elementalType == ElementalType.Ice)
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
using System;
using System.Collections;
using Builds;
using Infrastructure;
using TMPro;
using UnityEngine;

namespace Units
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyModel _enemyModel;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _status;
        [SerializeField] private UnitMover _unitMover;

        private PathModel _pathModel;
        private UnitHealth _unitHealth;
        private ElementalEffects _elementalEffects;
        
        private Coroutine _coroutine;

        public void Initialize(in PathModel pathModel)
        {
            _pathModel = pathModel;
            _unitHealth = new UnitHealth(_enemyModel.MaximumHealth, _enemyModel.ElementalResist);
            _elementalEffects = new ElementalEffects();

            _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
            _enemyModel.CurrentHealth = _enemyModel.MaximumHealth;

            _unitMover.Initialize(_enemyModel.CurrentMoveSpeed, _pathModel.WayPoints);
            _unitMover.MoveToNextPoint();
            
            DisplayHealth();
            DisplayDebuffs();
        }
    
        public void TakeDamage(CastItemModel castItemModel)
        {
            _unitHealth.TakeDamage(castItemModel.Damage, castItemModel.ElementalType);
            _elementalEffects.TakeEffect(castItemModel.ElementalType);
            
            if (_unitHealth.CurrentValue <= 0)
            {
                DestroyEnemy();
                return;
            }
        
            StartEffectUpdates();
            UpdateUnitModel();
            DisplayHealth();
            DisplayDebuffs();
        }

        private void StartEffectUpdates()
        {
            StopEffectUpdates();
            
            if (_elementalEffects.ActiveDebuffs.Count != 0)
            {
                _coroutine = StartCoroutine(TickTimeEffectUpdates());
            }
        }

        private void UpdateUnitModel()
        {
            _enemyModel.CurrentHealth.Value = _unitHealth.CurrentValue;
            _enemyModel.DebuffModels.Value = _elementalEffects.ActiveDebuffs;
        }

        private void DisplayHealth()
        {
            if (_enemyModel.CurrentHealth >= 0)
            {
                _health.text = $"{_enemyModel.CurrentHealth}/{_enemyModel.MaximumHealth.ToString()}";
            }
        }

        private void DisplayDebuffs()
        {
            _status.text = String.Empty;

            if (_enemyModel.DebuffModels.Value.Count == 0)
            {
                return;
            }

            foreach (var debuffModel in _enemyModel.DebuffModels.Value)
            {
                _status.text = $"{_status.text} ({debuffModel.DebuffType})";
            }
        }

        private void StopEffectUpdates()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator TickTimeEffectUpdates()
        {
            while (_elementalEffects.ActiveDebuffs.Count > 0)
            {
                yield return new WaitForSeconds(GlobalParams.TickTime);
                _elementalEffects.UpdateDuration(GlobalParams.TickTime);
                TakeEffects();
            }
        
            _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
        }
        
        private void DestroyEnemy()
        {
            StopEffectUpdates();
            EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleDestroy());
            Destroy(gameObject);
        }
        
        private void TakeEffects()
        {
            foreach (var debuff in _elementalEffects.ActiveDebuffs)
            {
                if (debuff.DebuffType == DebuffType.Burning)
                {
                    _unitHealth.TakeDamage(GlobalParams.BurningDamage, ElementalType.Fire);
                }
                else if (debuff.DebuffType == DebuffType.Intoxication)
                {
                    _unitHealth.TakeDamage(GlobalParams.IntoxicationDamage, ElementalType.Poison);
                }
                else if (debuff.DebuffType == DebuffType.Slow)
                {
                    _enemyModel.CurrentMoveSpeed.Value = _enemyModel.MoveSpeed - GlobalParams.IceSlow;
                    _unitHealth.TakeDamage(GlobalParams.BurningDamage, ElementalType.Ice);
                }
                else if (debuff.DebuffType == DebuffType.Frozen)
                {
                    _enemyModel.CurrentMoveSpeed.Value = 0;
                    _unitHealth.TakeDamage(GlobalParams.BurningDamage, ElementalType.Ice);
                }
            }
            
            UpdateUnitModel();
            DisplayHealth();
            DisplayDebuffs();
        }
        
        // private void RemoveEffects(List<DebuffModel> debuffModels)
        // {
        //     foreach (var debuffModel in debuffModels)
        //     {
        //         if (debuffModel.DebuffType == DebuffType.Slow)
        //         {
        //             _enemyModel.CurrentMoveSpeed.Value = _enemyModel.MoveSpeed;
        //         }
        //         else if (debuffModel.DebuffType == DebuffType.Frozen)
        //         {
        //             _enemyModel.CurrentMoveSpeed.Value = _enemyModel.MoveSpeed;
        //         }
        //     }
        // }
    }
}
using System;
using System.Collections;
using System.Linq;
using Builds;
using GameLogic.Navigation;
using Infrastructure;
using TMPro;
using UnityEngine;

namespace Units
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyModel _enemyModel;
        [Space] 
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
            _elementalEffects = new ElementalEffects(_enemyModel.ElementalResist);

            _unitMover.Initialize(_enemyModel.BaseMoveSpeed, _pathModel.WayPoints);
            _unitMover.MoveToNextPoint();

            DisplayHealth();
            DisplayDebuffs();
        }

        public void SetWayPoint(Vector3 nextWayPoint) => _unitMover.SetWayPoint(nextWayPoint);


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
            DisplayHealth();
            DisplayDebuffs();
        }

        private void StartEffectUpdates()
        {
            StopEffectUpdates();

            if (_elementalEffects.ActiveDebuffs.Count != 0)
            {
                _coroutine = StartCoroutine(EffectUpdatesTickTime());
            }
        }

        private void DisplayHealth()
        {
            if (_unitHealth.CurrentValue >= 0)
            {
                _health.text = $"{_unitHealth.CurrentValue}/{_enemyModel.MaximumHealth}";
            }
        }

        private void DisplayDebuffs()
        {
            _status.text = String.Empty;

            if (_elementalEffects.ActiveDebuffs.Count == 0)
            {
                return;
            }

            foreach (var debuff in _elementalEffects.ActiveDebuffs)
            {
                _status.text = $"{_status.text} ({debuff.DebuffType})";
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

        private IEnumerator EffectUpdatesTickTime()
        {
            while (_elementalEffects.ActiveDebuffs.Count > 0)
            {
                yield return new WaitForSeconds(GlobalParams.TickTime);
                _elementalEffects.UpdateDuration(GlobalParams.TickTime);
                UpdateEffects();
            }
        }

        private void DestroyEnemy()
        {
            StopEffectUpdates();
            EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleEnemyDestroy());
            Destroy(gameObject);
        }

        private void UpdateEffects()
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
                    var currentMoveSpeed = _enemyModel.BaseMoveSpeed - GlobalParams.IceSlow;
                    _unitMover.SetMoveSpeed(currentMoveSpeed);
                }
                else if (debuff.DebuffType == DebuffType.Frozen)
                {
                    var currentMoveSpeed = 0;
                    _unitMover.SetMoveSpeed(currentMoveSpeed);
                }
            }

            if (_unitHealth.CurrentValue <= 0)
            {
                DestroyEnemy();
                return;
            }

            RemoveSpeedReduction();
            DisplayHealth();
            DisplayDebuffs();
        }

        private void RemoveSpeedReduction()
        {
            var canResetMoveSpeed = true;

            foreach (var debuff in _elementalEffects.ActiveDebuffs.Where(debuff =>
                         debuff.DebuffType is DebuffType.Slow or DebuffType.Frozen))
            {
                canResetMoveSpeed = false;
            }

            if (canResetMoveSpeed == true)
            {
                _unitMover.SetMoveSpeed(_enemyModel.BaseMoveSpeed);
            }
        }
    }
}
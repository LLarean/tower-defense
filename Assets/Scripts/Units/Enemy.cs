using System;
using System.Collections;
using System.Collections.Generic;
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

        private UnitHealth _unitHealth;
        private PathModel _pathModel;
        private Coroutine _coroutine;

        public void Initialize(in PathModel pathModel)
        {
            _pathModel = pathModel;
        
            _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
            _enemyModel.CurrentHealth = _enemyModel.MaximumHealth;
        
            DisplayHealth();
            DisplayDebuffs();
        
            _unitMover.Initialize(_enemyModel.CurrentMoveSpeed, _pathModel.WayPoints);
            _unitMover.MoveToNextPoint();

            _unitHealth = new UnitHealth(_enemyModel);
        
            _enemyModel.CurrentHealth.ValueChanged += ChangeHealth;
            _enemyModel.CurrentMoveSpeed.ValueChanged += ChangeMoveSpeed;
            _enemyModel.DebuffModels.ValueChanged += ChangeDebuffs;
        }
    
        public void TakeDamage(CastItemModel castItemModel)
        {
            _unitHealth.TakeDamage(castItemModel);
            
            if (_enemyModel.CurrentHealth <= 0)
            {
                DestroyEnemy();
                return;
            }
        
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_enemyModel.DebuffModels.Value.Count != 0)
            {
                _coroutine = StartCoroutine(TickState());
            }
        }

        private void ChangeHealth(int current, int previous)
        {
            DisplayHealth();
            DestroyEnemy();
        }

        private void DisplayHealth()
        {
            if (_enemyModel.CurrentHealth >= 0)
            {
                _health.text = $"{_enemyModel.CurrentHealth}/{_enemyModel.MaximumHealth.ToString()}";
            }
        }
    
        private void ChangeMoveSpeed(float current, float previous)
        {
            _unitMover.ChangeSpeed(_enemyModel.CurrentMoveSpeed);
        }

        private void ChangeDebuffs(List<DebuffModel> current, List<DebuffModel> previous)
        {
            DisplayDebuffs();
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

        private IEnumerator TickState()
        {
            while (_enemyModel.DebuffModels.Value.Count > 0)
            {
                yield return new WaitForSeconds(GlobalParams.TickTime);
                _unitHealth.UpdateDebuffsDuration();
            }
        
            _enemyModel.CurrentMoveSpeed = _enemyModel.MoveSpeed;
        }
        
        private void DestroyEnemy()
        {
            if (_enemyModel.CurrentHealth <= 0)
            {
                EventBus.RaiseEvent<IEnemyHandler>(enemyHandler => enemyHandler.HandleDestroy());
                Destroy(gameObject);
            }
        }
    }
}
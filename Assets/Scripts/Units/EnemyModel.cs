using System;
using System.Collections.Generic;
using Builds;
using UnityEngine;

namespace Units
{
    [Serializable]
    public class EnemyModel
    {
        [SerializeField] [Range(1, 1000)] private int _maximumHealth = 100;
        [SerializeField] [Range(0, 100)] private float _moveSpeed = 20f;
        [SerializeField] private ElementalType elementalResist;
    
        [HideInInspector] public Observable<int> CurrentHealth;
        [HideInInspector] public Observable<float> CurrentMoveSpeed;
        [HideInInspector] public Observable<List<DebuffModel>> DebuffModels = new List<DebuffModel>();
    
        public int MaximumHealth => _maximumHealth;
        public float MoveSpeed => _moveSpeed;
        public ElementalType ElementalResist => elementalResist;
    }
}
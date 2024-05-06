using System;
using Towers;
using UnityEngine;

namespace Units
{
    [Serializable]
    public class EnemyModel
    {
        [SerializeField] [Range(1, 1000)] private int _maximumHealth = 100;
        [SerializeField] [Range(0, 100)] private float _baseMoveSpeed = 20f;
        [SerializeField] private ElementalType _elementalResist = ElementalType.None;
    
        public int MaximumHealth => _maximumHealth;
        public float BaseMoveSpeed => _baseMoveSpeed;
        public ElementalType ElementalResist => _elementalResist;
    }
}
using System;
using Game.Core.Towers;
using UnityEngine;

namespace Game.Stage.Config
{
    [Serializable]
    public struct GradeData
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _potency;
        [SerializeField] private float _duration;
        [SerializeField] private float _range;
        [SerializeField] private float _cooldown;
        [SerializeField] private int _cost;

        public Grade Grade() => new Grade(_damage, _potency, _duration, _range, _cooldown, _cost);
    }
}

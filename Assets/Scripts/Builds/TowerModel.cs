﻿using System;
using UnityEngine;

namespace Builds
{
    [Serializable]
    public class TowerModel
    {
        public ElementalType ElementalType;
        [Range(0, 10)] public float AttackSpeed;
        [Range(0, 200)] public int Damage;
    }
}
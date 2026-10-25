using System;
using UnityEngine;

namespace Game.Stage.Config
{
    [Serializable]
    public struct LetteringData
    {
        [SerializeField] private int _count;
        [SerializeField] private float _life;
        [SerializeField] private float _rise;
        [SerializeField] private float _pop;

        public int Count => _count;

        public float Life => _life;

        public float Rise => _rise;

        public float Pop => _pop;
    }
}

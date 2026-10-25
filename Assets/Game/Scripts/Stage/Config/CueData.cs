using System;
using UnityEngine;

namespace Game.Stage.Config
{
    [Serializable]
    public struct CueData
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField, Range(0f, 1f)] private float _volume;
        [SerializeField] private float _gap;

        public bool Silent => _clip == null;

        public AudioClip Clip => _clip;

        public float Volume => _volume;

        public float Gap => _gap;
    }
}

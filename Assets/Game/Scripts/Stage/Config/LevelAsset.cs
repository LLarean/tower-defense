using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Level")]
    public sealed class LevelAsset : ScriptableObject
    {
        [SerializeField] private int _gold = 300;
        [SerializeField] private int _lives = 20;
        [SerializeField] private float _rest = 15f;
        [SerializeField] private TowerAsset[] _arsenal = new TowerAsset[0];
        [SerializeField] private WaveData[] _waves = new WaveData[0];

        public int Gold => _gold;

        public int Lives => _lives;

        public float Rest => _rest;

        public IReadOnlyList<TowerAsset> Arsenal => _arsenal;

        public IReadOnlyList<WaveData> Waves => _waves;
    }
}

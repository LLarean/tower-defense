using System;
using Game.Core.Waves;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Difficulty")]
    public sealed class DifficultyAsset : ScriptableObject
    {
        [Serializable]
        private struct Tier
        {
            [SerializeField] private string _title;
            [SerializeField, TextArea] private string _summary;
            [SerializeField] private float _health;
            [SerializeField] private float _bounty;
            [SerializeField] private float _gold;
            [SerializeField] private float _lives;

            public string Title => _title;

            public string Summary => _summary;

            public float Health => _health;

            public float Bounty => _bounty;

            public float Gold => _gold;

            public float Lives => _lives;
        }

        [SerializeField] private Tier[] _tiers = new Tier[0];
        [SerializeField] private int _default = 1;
        [SerializeField] private float _crowding = 0.7f;

        public int Count => _tiers.Length;

        public int Default => _default;

        public string Title(int tier) => _tiers[Clamp(tier)].Title;

        public string Summary(int tier) => _tiers[Clamp(tier)].Summary;

        public Threat Threat(int tier, int players) => new Threat(_tiers[Clamp(tier)].Health, _tiers[Clamp(tier)].Bounty, _crowding, players);

        public int Gold(int tier, int gold) => Mathf.RoundToInt(gold * _tiers[Clamp(tier)].Gold);

        public int Lives(int tier, int lives) => Mathf.Max(1, Mathf.RoundToInt(lives * _tiers[Clamp(tier)].Lives));

        private int Clamp(int tier) => Mathf.Clamp(tier, 0, _tiers.Length - 1);
    }
}

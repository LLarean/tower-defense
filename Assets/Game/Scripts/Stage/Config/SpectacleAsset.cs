using System;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Stage.Effects;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Spectacle")]
    public sealed class SpectacleAsset : ScriptableObject
    {
        [Serializable]
        private struct Reaction
        {
            [SerializeField] private ReactionKind _kind;
            [SerializeField] private Burst _burst;
            [SerializeField] private float _shake;

            public ReactionKind Kind => _kind;

            public Burst Burst => _burst;

            public float Shake => _shake;
        }

        [Serializable]
        private struct Mark
        {
            [SerializeField] private Condition _condition;
            [SerializeField, ColorUsage(false, true)] private Color _color;

            public Condition Condition => _condition;

            public Color Color => _color;
        }

        [SerializeField] private Reaction[] _reactions = new Reaction[0];
        [SerializeField] private Mark[] _marks = new Mark[0];
        [SerializeField] private Burst _death;
        [SerializeField] private Bolt _bolt;
        [SerializeField] private float _bolting = 0.15f;

        public Burst Death => _death;

        public Bolt Bolt => _bolt;

        public float Bolting => _bolting;

        public int Marks => _marks.Length;

        public bool TryReaction(ReactionKind kind, out Burst burst, out float shake)
        {
            foreach (var reaction in _reactions)
            {
                if (reaction.Kind == kind)
                {
                    burst = reaction.Burst;
                    shake = reaction.Shake;
                    return burst != null;
                }
            }
            burst = null;
            shake = 0f;
            return false;
        }

        public Condition Condition(int mark) => _marks[mark].Condition;

        public Color Color(int mark) => _marks[mark].Color;
    }
}

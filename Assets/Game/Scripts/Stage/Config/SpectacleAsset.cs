using System;
using Game.Core.Elements;
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
            [SerializeField] private Color _color;

            public ReactionKind Kind => _kind;

            public Burst Burst => _burst;

            public float Shake => _shake;

            public Color Color => _color;
        }

        [Serializable]
        private struct Tint
        {
            [SerializeField] private Element _element;
            [SerializeField] private Color _color;

            public Element Element => _element;

            public Color Color => _color;
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
        [SerializeField] private Tint[] _tints = new Tint[0];
        [SerializeField] private LetteringData _titles;
        [SerializeField] private LetteringData _numbers;
        [SerializeField] private float _tallying = 0.3f;
        [SerializeField] private float _noticeable = 1f;
        [SerializeField] private float _heavy = 50f;
        [SerializeField] private float _hush = 6f;

        public Burst Death => _death;

        public Bolt Bolt => _bolt;

        public float Bolting => _bolting;

        public int Marks => _marks.Length;

        public LetteringData Titles => _titles;

        public LetteringData Numbers => _numbers;

        public float Tallying => _tallying;

        public float Noticeable => _noticeable;

        public float Heavy => _heavy;

        public float Hush => _hush;

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

        public Color Color(ReactionKind kind)
        {
            foreach (var reaction in _reactions)
            {
                if (reaction.Kind == kind)
                {
                    return reaction.Color;
                }
            }
            return UnityEngine.Color.white;
        }

        public Color Color(Element element)
        {
            foreach (var tint in _tints)
            {
                if (tint.Element == element)
                {
                    return tint.Color;
                }
            }
            return UnityEngine.Color.white;
        }

        public Condition Condition(int mark) => _marks[mark].Condition;

        public Color Color(int mark) => _marks[mark].Color;
    }
}

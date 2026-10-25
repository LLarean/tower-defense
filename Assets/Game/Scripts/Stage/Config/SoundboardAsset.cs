using System;
using Game.Core.Elements;
using Game.Core.Reactions;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Soundboard")]
    public sealed class SoundboardAsset : ScriptableObject
    {
        [Serializable]
        private struct ElementCue
        {
            [SerializeField] private Element _element;
            [SerializeField] private CueData _cue;

            public Element Element => _element;

            public CueData Cue => _cue;
        }

        [Serializable]
        private struct ReactionCue
        {
            [SerializeField] private ReactionKind _kind;
            [SerializeField] private CueData _cue;

            public ReactionKind Kind => _kind;

            public CueData Cue => _cue;
        }

        [Header("Interface")]
        [SerializeField] private CueData _click;
        [Header("Garrison")]
        [SerializeField] private CueData _build;
        [SerializeField] private CueData _upgrade;
        [SerializeField] private CueData _sell;
        [Header("Battle")]
        [SerializeField] private CueData _wave;
        [SerializeField] private CueData _leak;
        [SerializeField] private CueData _death;
        [SerializeField] private CueData _victory;
        [SerializeField] private CueData _defeat;
        [SerializeField] private ElementCue[] _shots = new ElementCue[0];
        [SerializeField] private ReactionCue[] _reactions = new ReactionCue[0];
        [Header("Music")]
        [SerializeField] private AudioClip _menu;
        [SerializeField] private AudioClip _battle;
        [SerializeField, Range(0f, 1f)] private float _music = 0.35f;
        [SerializeField] private int _voices = 24;

        public CueData Click => _click;

        public CueData Build => _build;

        public CueData Upgrade => _upgrade;

        public CueData Sell => _sell;

        public CueData Wave => _wave;

        public CueData Leak => _leak;

        public CueData Death => _death;

        public CueData Victory => _victory;

        public CueData Defeat => _defeat;

        public AudioClip Menu => _menu;

        public AudioClip Battle => _battle;

        public float Music => _music;

        public int Voices => _voices;

        public CueData Shot(Element element)
        {
            foreach (var shot in _shots)
            {
                if (shot.Element == element)
                {
                    return shot.Cue;
                }
            }
            return default;
        }

        public CueData Reaction(ReactionKind kind)
        {
            foreach (var reaction in _reactions)
            {
                if (reaction.Kind == kind)
                {
                    return reaction.Cue;
                }
            }
            return default;
        }
    }
}

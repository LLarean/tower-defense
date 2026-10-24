using System;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Lore")]
    public sealed class LoreAsset : ScriptableObject
    {
        [Serializable]
        private struct Essence
        {
            [SerializeField] private Element _element;
            [SerializeField] private string _title;

            public Element Element => _element;

            public string Title => _title;
        }

        [Serializable]
        private struct State
        {
            [SerializeField] private Condition _condition;
            [SerializeField] private string _title;
            [SerializeField] private string _effect;

            public Condition Condition => _condition;

            public string Title => _title;

            public string Effect => _effect;
        }

        [Serializable]
        private struct Tale
        {
            [SerializeField] private ReactionKind _kind;
            [SerializeField] private string _title;
            [SerializeField, TextArea] private string _text;

            public ReactionKind Kind => _kind;

            public string Title => _title;

            public string Text => _text;
        }

        [SerializeField] private Essence[] _elements = new Essence[0];
        [SerializeField] private State[] _states = new State[0];
        [SerializeField] private Tale[] _reactions = new Tale[0];

        public string Title(Element element)
        {
            foreach (var essence in _elements)
            {
                if (essence.Element == element)
                {
                    return essence.Title;
                }
            }
            return element.ToString();
        }

        public string Title(Condition condition)
        {
            foreach (var state in _states)
            {
                if (state.Condition == condition)
                {
                    return state.Title;
                }
            }
            return condition.ToString();
        }

        public string Effect(Condition condition)
        {
            foreach (var state in _states)
            {
                if (state.Condition == condition)
                {
                    return state.Effect;
                }
            }
            return string.Empty;
        }

        public string Title(ReactionKind kind)
        {
            foreach (var tale in _reactions)
            {
                if (tale.Kind == kind)
                {
                    return tale.Title;
                }
            }
            return kind.ToString();
        }

        public string Text(ReactionKind kind)
        {
            foreach (var tale in _reactions)
            {
                if (tale.Kind == kind)
                {
                    return tale.Text;
                }
            }
            return string.Empty;
        }
    }
}

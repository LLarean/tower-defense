using System;
using System.Collections.Generic;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Stage.Config;
using Game.Stage.Speech;
using UnityEngine;

namespace Game.Stage.Hud
{
    public sealed class Glossary
    {
        private static readonly Color Plain = new Color(0.85f, 0.87f, 0.9f);

        private readonly LoreAsset _lore;
        private readonly Lexicon _lexicon;
        private readonly Dictionary<Element, string> _elements = new Dictionary<Element, string>();
        private readonly Dictionary<Condition, string> _conditions = new Dictionary<Condition, string>();
        private readonly Dictionary<ReactionKind, string> _reactions = new Dictionary<ReactionKind, string>();

        public Glossary(LoreAsset lore, SpectacleAsset spectacle, Lexicon lexicon)
        {
            _lore = lore;
            _lexicon = lexicon;
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                _elements[element] = Paint(lexicon.Say(lore.Title(element)), spectacle.Color(element));
                _conditions[Statuses.MarkOf(element)] = Paint(lexicon.Say(lore.Title(Statuses.MarkOf(element))), spectacle.Color(element));
            }
            foreach (var condition in Statuses.All)
            {
                if (!_conditions.ContainsKey(condition))
                {
                    _conditions[condition] = Paint(lexicon.Say(lore.Title(condition)), Plain);
                }
            }
            foreach (ReactionKind kind in Enum.GetValues(typeof(ReactionKind)))
            {
                _reactions[kind] = Paint(lexicon.Say(lore.Title(kind)), spectacle.Color(kind));
            }
        }

        public string Element(Element element) => _elements[element];

        public string Condition(Condition condition) => _conditions[condition];

        public string Reaction(ReactionKind kind) => _reactions[kind];

        public string Effect(Condition condition) => _lexicon.Say(_lore.Effect(condition));

        private static string Paint(string text, Color color) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }
}

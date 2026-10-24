using System;
using System.Collections.Generic;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Stage.Config;

namespace Game.Stage.Hud
{
    public sealed class Recipes
    {
        private readonly List<(string First, string Second, ReactionKind Kind)> _all = new List<(string, string, ReactionKind)>();
        private readonly List<(Element Element, string Partner, ReactionKind Kind)> _pairs = new List<(Element, string, ReactionKind)>();

        public Recipes(Chemistry chemistry, LoreAsset lore)
        {
            var sources = new Dictionary<Condition, Element>();
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                sources[Statuses.MarkOf(element)] = element;
            }
            var seen = new HashSet<ReactionKind>();
            foreach (var (condition, element, kind) in chemistry.Entries)
            {
                var first = sources.TryGetValue(condition, out var origin) ? lore.Title(origin) : lore.Title(condition);
                _pairs.Add((element, first, kind));
                if (sources.ContainsKey(condition))
                {
                    _pairs.Add((origin, lore.Title(element), kind));
                }
                if (seen.Add(kind))
                {
                    _all.Add((first, lore.Title(element), kind));
                }
            }
        }

        public IReadOnlyList<(string First, string Second, ReactionKind Kind)> All => _all;

        public IEnumerable<(string Partner, ReactionKind Kind)> With(Element element)
        {
            var seen = new HashSet<ReactionKind>();
            foreach (var (owner, partner, kind) in _pairs)
            {
                if (owner == element && seen.Add(kind))
                {
                    yield return (partner, kind);
                }
            }
        }
    }
}

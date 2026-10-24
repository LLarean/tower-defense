using System;
using System.Collections.Generic;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class Chemistry
    {
        private readonly IReaction[,] _table = new IReaction[
            Enum.GetValues(typeof(Condition)).Length,
            Enum.GetValues(typeof(Element)).Length];

        public Chemistry With(Condition condition, Element element, IReaction reaction)
        {
            _table[(int)condition, (int)element] = reaction;
            return this;
        }

        public Chemistry With(Element first, Element second, IReaction reaction)
        {
            return With(Statuses.MarkOf(first), second, reaction)
                .With(Statuses.MarkOf(second), first, reaction);
        }

        public IEnumerable<(Condition Condition, Element Element, ReactionKind Kind)> Entries
        {
            get
            {
                for (var condition = 0; condition < _table.GetLength(0); condition++)
                {
                    for (var element = 0; element < _table.GetLength(1); element++)
                    {
                        var reaction = _table[condition, element];
                        if (reaction != null)
                        {
                            yield return ((Condition)condition, (Element)element, reaction.Kind);
                        }
                    }
                }
            }
        }

        public bool TryFind(Condition condition, Element element, out IReaction reaction)
        {
            reaction = _table[(int)condition, (int)element];
            return reaction != null;
        }
    }
}

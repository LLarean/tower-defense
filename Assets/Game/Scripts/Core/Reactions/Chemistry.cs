using System;
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

        public bool TryFind(Condition condition, Element element, out IReaction reaction)
        {
            reaction = _table[(int)condition, (int)element];
            return reaction != null;
        }
    }
}

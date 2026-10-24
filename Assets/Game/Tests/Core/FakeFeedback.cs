using System.Collections.Generic;
using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;

namespace Game.Core.Tests
{
    public sealed class FakeFeedback : IFeedback
    {
        public List<ReactionKind> Reactions { get; } = new List<ReactionKind>();

        public List<Monster> Dead { get; } = new List<Monster>();

        public List<Monster> Gone { get; } = new List<Monster>();

        public int Chains { get; private set; }

        public void Wounded(Monster monster, float amount, Element element)
        {
        }

        public void Reacted(ReactionKind kind, Vector3 position, float radius) => Reactions.Add(kind);

        public void Chained(Vector3 from, Vector3 to) => Chains++;

        public void Zoned(ReactionKind kind, Vector3 position, float radius, float seconds) => Reactions.Add(kind);

        public void Died(Monster monster) => Dead.Add(monster);

        public void Escaped(Monster monster) => Gone.Add(monster);
    }
}

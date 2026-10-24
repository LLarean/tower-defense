using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class Overload : IReaction
    {
        private readonly float _radius;
        private readonly float _multiplier;
        private readonly float _push;
        private readonly List<Monster> _near = new List<Monster>();

        public Overload(float radius, float multiplier, float push)
        {
            _radius = radius;
            _multiplier = multiplier;
            _push = push;
        }

        public ReactionKind Kind => ReactionKind.Overload;

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            var origin = target.Position;
            arena.Horde.Near(origin, _radius, _near);
            foreach (var monster in _near)
            {
                monster.Wound(hit.Damage * _multiplier, Element.Lightning, arena);
                monster.Push(_push);
            }
            arena.Feedback.Reacted(Kind, origin, _radius);
        }
    }
}

using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class GasExplosion : IReaction
    {
        private readonly float _radius;
        private readonly float _multiplier;
        private readonly float _seconds;
        private readonly List<Monster> _near = new List<Monster>();

        public GasExplosion(float radius, float multiplier, float seconds)
        {
            _radius = radius;
            _multiplier = multiplier;
            _seconds = seconds;
        }

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            var origin = target.Position;
            arena.Horde.Near(origin, _radius, _near);
            foreach (var monster in _near)
            {
                monster.Wound(hit.Damage * _multiplier, Element.Fire, arena);
                monster.Inflict(Condition.Burning, hit.Potency, _seconds);
            }
            arena.Feedback.Reacted(ReactionKind.GasExplosion, origin, _radius);
        }
    }
}

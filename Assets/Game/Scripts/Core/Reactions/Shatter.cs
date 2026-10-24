using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class Shatter : IReaction
    {
        private readonly float _radius;
        private readonly float _multiplier;
        private readonly float _splash;
        private readonly List<Monster> _near = new List<Monster>();

        public Shatter(float radius, float multiplier, float splash)
        {
            _radius = radius;
            _multiplier = multiplier;
            _splash = splash;
        }

        public ReactionKind Kind => ReactionKind.Shatter;

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            var damage = hit.Damage * _multiplier;
            var origin = target.Position;
            target.Wound(damage, Element.Earth, arena);
            arena.Horde.Near(origin, _radius, _near);
            foreach (var monster in _near)
            {
                if (monster != target)
                {
                    monster.Wound(damage * _splash, Element.Earth, arena);
                }
            }
            arena.Feedback.Reacted(Kind, origin, _radius);
        }
    }
}

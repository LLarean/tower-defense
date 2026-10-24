using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class Electrocute : IReaction
    {
        private readonly float _radius;
        private readonly float _multiplier;
        private readonly List<Monster> _near = new List<Monster>();

        public Electrocute(float radius, float multiplier)
        {
            _radius = radius;
            _multiplier = multiplier;
        }

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            var damage = hit.Damage * _multiplier;
            var origin = target.Position;
            target.Wound(damage, Element.Lightning, arena);
            arena.Horde.Near(origin, _radius, _near);
            foreach (var monster in _near)
            {
                if (monster == target || !monster.Has(Condition.Wet))
                {
                    continue;
                }
                monster.Cure(Condition.Wet);
                monster.Wound(damage, Element.Lightning, arena);
                arena.Feedback.Chained(origin, monster.Position);
            }
            arena.Feedback.Reacted(ReactionKind.Electrocute, origin, _radius);
        }
    }
}

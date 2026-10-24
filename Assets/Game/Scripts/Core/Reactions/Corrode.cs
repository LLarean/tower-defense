using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class Corrode : IReaction
    {
        private readonly ReactionKind _kind;
        private readonly float _radius;
        private readonly float _shred;
        private readonly float _seconds;
        private readonly List<Monster> _near = new List<Monster>();

        public Corrode(ReactionKind kind, float radius, float shred, float seconds)
        {
            _kind = kind;
            _radius = radius;
            _shred = shred;
            _seconds = seconds;
        }

        public ReactionKind Kind => _kind;

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            var origin = target.Position;
            target.Inflict(Condition.Corroded, _shred, _seconds);
            arena.Horde.Near(origin, _radius, _near);
            foreach (var monster in _near)
            {
                monster.Inflict(Condition.Corroded, _shred, _seconds);
            }
            arena.Feedback.Reacted(_kind, origin, _radius);
        }
    }
}

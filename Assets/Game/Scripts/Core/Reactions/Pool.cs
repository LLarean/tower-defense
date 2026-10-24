using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Zones;

namespace Game.Core.Reactions
{
    public sealed class Pool : IReaction
    {
        private readonly ReactionKind _kind;
        private readonly float _radius;
        private readonly float _seconds;
        private readonly IZoneEffect _effect;

        public Pool(ReactionKind kind, float radius, float seconds, IZoneEffect effect)
        {
            _kind = kind;
            _radius = radius;
            _seconds = seconds;
            _effect = effect;
        }

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            var area = new Circle(target.Position, _radius);
            arena.Ground.Add(new Zone(area, _seconds, _effect));
            arena.Feedback.Zoned(_kind, area.Center, _radius, _seconds);
        }
    }
}

using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Zones
{
    public sealed class Zone
    {
        private readonly Circle _area;
        private readonly IZoneEffect _effect;
        private float _remaining;

        public Zone(Circle area, float seconds, IZoneEffect effect)
        {
            _area = area;
            _remaining = seconds;
            _effect = effect;
        }

        public bool Expired => _remaining <= 0f;

        public void Tick(float seconds, Arena arena, List<Monster> inside)
        {
            arena.Horde.Near(_area.Center, _area.Radius, inside);
            foreach (var monster in inside)
            {
                _effect.Apply(monster, seconds, arena);
            }
            _remaining -= seconds;
        }
    }
}

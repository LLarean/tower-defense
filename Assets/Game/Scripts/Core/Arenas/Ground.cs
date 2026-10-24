using System.Collections.Generic;
using Game.Core.Monsters;
using Game.Core.Zones;

namespace Game.Core.Arenas
{
    public sealed class Ground
    {
        private readonly List<Zone> _zones = new List<Zone>();
        private readonly List<Monster> _inside = new List<Monster>();

        public void Add(Zone zone) => _zones.Add(zone);

        public void Tick(float seconds, Arena arena)
        {
            for (var index = _zones.Count - 1; index >= 0; index--)
            {
                var zone = _zones[index];
                zone.Tick(seconds, arena, _inside);
                if (zone.Expired)
                {
                    _zones.RemoveAt(index);
                }
            }
        }
    }
}

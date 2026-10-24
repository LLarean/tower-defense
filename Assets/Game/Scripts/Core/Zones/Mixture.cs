using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Zones
{
    public sealed class Mixture : IZoneEffect
    {
        private readonly IZoneEffect[] _effects;

        public Mixture(params IZoneEffect[] effects)
        {
            _effects = effects;
        }

        public void Apply(Monster monster, float seconds, Arena arena)
        {
            foreach (var effect in _effects)
            {
                effect.Apply(monster, seconds, arena);
            }
        }
    }
}

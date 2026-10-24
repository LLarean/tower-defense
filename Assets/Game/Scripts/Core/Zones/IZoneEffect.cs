using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Zones
{
    public interface IZoneEffect
    {
        void Apply(Monster monster, float seconds, Arena arena);
    }
}

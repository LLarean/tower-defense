using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Zones
{
    public sealed class Scald : IZoneEffect
    {
        private readonly Element _element;
        private readonly float _perSecond;

        public Scald(Element element, float perSecond)
        {
            _element = element;
            _perSecond = perSecond;
        }

        public void Apply(Monster monster, float seconds, Arena arena) => monster.Wound(_perSecond * seconds, _element, arena);
    }
}

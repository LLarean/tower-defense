using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Zones
{
    public sealed class Affliction : IZoneEffect
    {
        private const float Linger = 0.5f;

        private readonly Condition _condition;
        private readonly float _potency;

        public Affliction(Condition condition, float potency)
        {
            _condition = condition;
            _potency = potency;
        }

        public void Apply(Monster monster, float seconds, Arena arena) => monster.Inflict(_condition, _potency, Linger);
    }
}

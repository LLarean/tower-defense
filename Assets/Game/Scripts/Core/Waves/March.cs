using Game.Core.Arenas;
using Game.Core.Monsters;
using Game.Core.Paths;

namespace Game.Core.Waves
{
    public sealed class March
    {
        private readonly Squad _squad;
        private readonly Route _route;
        private float _elapsed;
        private int _released;

        public March(Squad squad, Route route)
        {
            _squad = squad;
            _route = route;
        }

        public bool Done => _released >= _squad.Count;

        public void Tick(float seconds, Horde horde)
        {
            _elapsed += seconds;
            while (!Done && _elapsed >= _squad.Delay + _released * _squad.Interval)
            {
                horde.Add(new Monster(_squad.Breed, _route));
                _released++;
            }
        }
    }
}

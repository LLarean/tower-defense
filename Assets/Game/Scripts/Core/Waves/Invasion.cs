using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Paths;

namespace Game.Core.Waves
{
    public sealed class Invasion
    {
        private readonly IReadOnlyList<Route> _routes;
        private readonly float _rest;
        private readonly Wave[] _waves;
        private readonly List<March> _marches = new List<March>();

        public Invasion(IReadOnlyList<Route> routes, float rest, params Wave[] waves)
        {
            _routes = routes;
            _rest = rest;
            _waves = waves;
        }

        public int Wave { get; private set; }

        public int Waves => _waves.Length;

        public float Countdown { get; private set; }

        public bool Exhausted => Wave >= _waves.Length && _marches.Count == 0;

        public void Summon()
        {
            if (Wave >= _waves.Length)
            {
                return;
            }
            foreach (var squad in _waves[Wave].Squads)
            {
                _marches.Add(new March(squad, _routes[squad.Route]));
            }
            Wave++;
            Countdown = _rest;
        }

        public void Tick(float seconds, Horde horde)
        {
            for (var index = _marches.Count - 1; index >= 0; index--)
            {
                _marches[index].Tick(seconds, horde);
                if (_marches[index].Done)
                {
                    _marches.RemoveAt(index);
                }
            }
            if (_marches.Count > 0 || Wave >= _waves.Length)
            {
                return;
            }
            Countdown -= seconds;
            if (Countdown <= 0f)
            {
                Summon();
                Tick(0f, horde);
            }
        }
    }
}

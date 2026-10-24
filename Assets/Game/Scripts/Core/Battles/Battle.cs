using Game.Core.Arenas;
using Game.Core.Economy;
using Game.Core.Towers;
using Game.Core.Waves;

namespace Game.Core.Battles
{
    public sealed class Battle
    {
        private readonly Lives _lives;

        public Battle(Arena arena, Invasion invasion, Garrison garrison, Lives lives)
        {
            Arena = arena;
            Invasion = invasion;
            Garrison = garrison;
            _lives = lives;
            Volley = new Volley();
        }

        public Arena Arena { get; }

        public Invasion Invasion { get; }

        public Garrison Garrison { get; }

        public Volley Volley { get; }

        public bool Started { get; private set; }

        public Outcome Outcome
        {
            get
            {
                if (_lives.Lost)
                {
                    return Outcome.Lost;
                }
                if (Started && Invasion.Exhausted && Arena.Horde.Monsters.Count == 0)
                {
                    return Outcome.Won;
                }
                return Outcome.Ongoing;
            }
        }

        public void Begin()
        {
            if (Started)
            {
                return;
            }
            Started = true;
            Invasion.Summon();
        }

        public void Tick(float seconds)
        {
            if (!Started || Outcome != Outcome.Ongoing)
            {
                return;
            }
            Invasion.Tick(seconds, Arena.Horde);
            Garrison.Tick(seconds, Arena, Volley);
            Volley.Tick(seconds, Arena);
            Arena.Tick(seconds);
        }
    }
}

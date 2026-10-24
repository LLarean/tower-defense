using System.Collections.Generic;

namespace Game.Core.Waves
{
    public sealed class Wave
    {
        private readonly Squad[] _squads;

        public Wave(params Squad[] squads)
        {
            _squads = squads;
        }

        public IReadOnlyList<Squad> Squads => _squads;
    }
}

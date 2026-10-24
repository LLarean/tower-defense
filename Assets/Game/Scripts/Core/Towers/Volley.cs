using System.Collections.Generic;
using Game.Core.Arenas;

namespace Game.Core.Towers
{
    public sealed class Volley
    {
        private readonly List<Missile> _missiles = new List<Missile>();

        public IReadOnlyList<Missile> Missiles => _missiles;

        public void Launch(Missile missile) => _missiles.Add(missile);

        public void Tick(float seconds, Arena arena)
        {
            for (var index = _missiles.Count - 1; index >= 0; index--)
            {
                var missile = _missiles[index];
                missile.Fly(seconds, arena);
                if (missile.Done)
                {
                    _missiles.RemoveAt(index);
                }
            }
        }
    }
}

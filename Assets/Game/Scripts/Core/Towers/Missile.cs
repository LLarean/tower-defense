using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Towers
{
    public sealed class Missile
    {
        private readonly Monster _target;
        private readonly Strike _strike;
        private Vector3 _aim;

        public Missile(Tower source, Monster target, Strike strike)
        {
            Source = source;
            Position = source.Position;
            _target = target;
            _strike = strike;
            _aim = target.Position;
        }

        public Tower Source { get; }

        public Vector3 Position { get; private set; }

        public bool Done { get; private set; }

        public void Fly(float seconds, Arena arena)
        {
            if (_target.Alive)
            {
                _aim = _target.Position;
            }
            var offset = _aim - Position;
            var step = Source.Spec.Speed * seconds;
            if (offset.Length() > step)
            {
                Position += Vector3.Normalize(offset) * step;
                return;
            }
            Position = _aim;
            Done = true;
            if (_target.Alive)
            {
                _strike.Land(_target, arena);
            }
        }
    }
}

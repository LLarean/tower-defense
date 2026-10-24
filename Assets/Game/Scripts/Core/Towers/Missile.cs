using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Towers
{
    public sealed class Missile
    {
        private readonly Monster _target;
        private readonly float _speed;
        private readonly Strike _strike;
        private Vector3 _aim;

        public Missile(Vector3 origin, Monster target, float speed, Strike strike)
        {
            Position = origin;
            _target = target;
            _speed = speed;
            _strike = strike;
            _aim = target.Position;
        }

        public Vector3 Position { get; private set; }

        public Strike Strike => _strike;

        public bool Done { get; private set; }

        public void Fly(float seconds, Arena arena)
        {
            if (_target.Alive)
            {
                _aim = _target.Position;
            }
            var offset = _aim - Position;
            var step = _speed * seconds;
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

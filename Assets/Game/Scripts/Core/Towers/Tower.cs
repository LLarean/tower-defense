using System.Collections.Generic;
using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Monsters;

namespace Game.Core.Towers
{
    public sealed class Tower
    {
        private const int RefundPercent = 70;

        private readonly TowerSpec _spec;
        private readonly List<Monster> _near = new List<Monster>();
        private float _cooldown;

        public Tower(int id, TowerSpec spec, Vector3 position, int owner)
        {
            Id = id;
            _spec = spec;
            Position = position;
            Owner = owner;
        }

        public int Id { get; }

        public TowerSpec Spec => _spec;

        public Vector3 Position { get; }

        public int Owner { get; }

        public int Level { get; private set; }

        public Grade Grade => _spec.Grade(Level);

        public bool Upgradable => Level + 1 < _spec.Levels;

        public int UpgradeCost => _spec.Grade(Level + 1).Cost;

        public int Worth
        {
            get
            {
                var worth = 0;
                for (var level = 0; level <= Level; level++)
                {
                    worth += _spec.Grade(level).Cost;
                }
                return worth;
            }
        }

        public int Refund => Worth * RefundPercent / 100;

        public void Upgrade() => Level++;

        public void Tick(float seconds, Arena arena, Volley volley)
        {
            _cooldown -= seconds;
            if (_cooldown > 0f || !TryAim(arena, out var target))
            {
                return;
            }
            var grade = Grade;
            volley.Launch(new Missile(this, target, new Strike(_spec.Elements, grade)));
            _cooldown = grade.Cooldown;
        }

        private bool TryAim(Arena arena, out Monster target)
        {
            arena.Horde.Near(Position, Grade.Range, _near);
            target = null;
            foreach (var monster in _near)
            {
                if (monster.Alive && (target == null || monster.Distance > target.Distance))
                {
                    target = monster;
                }
            }
            return target != null;
        }
    }
}

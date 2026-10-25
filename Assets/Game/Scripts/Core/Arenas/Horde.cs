using System.Collections.Generic;
using System.Numerics;
using Game.Core.Monsters;

namespace Game.Core.Arenas
{
    public sealed class Horde
    {
        private readonly List<Monster> _monsters = new List<Monster>();
        private readonly List<Monster> _recruits = new List<Monster>();
        private readonly Buckets _buckets;

        public Horde(float cell)
        {
            _buckets = new Buckets(cell);
        }

        public IReadOnlyList<Monster> Monsters => _monsters;

        public void Add(Monster monster)
        {
            _monsters.Add(monster);
            _buckets.Put(monster);
        }

        public void Recruit(Monster monster) => _recruits.Add(monster);

        public void Near(Vector3 point, float radius, List<Monster> found) => _buckets.Near(point, radius, found);

        public void Tick(float seconds, Arena arena)
        {
            foreach (var monster in _monsters)
            {
                monster.Tick(seconds, arena);
            }
            for (var index = _monsters.Count - 1; index >= 0; index--)
            {
                var monster = _monsters[index];
                if (!monster.Alive)
                {
                    arena.Feedback.Died(monster);
                    Remove(index);
                }
                else if (monster.Escaped)
                {
                    arena.Feedback.Escaped(monster);
                    Remove(index);
                }
            }
            _monsters.AddRange(_recruits);
            _recruits.Clear();
            _buckets.Clear();
            foreach (var monster in _monsters)
            {
                _buckets.Put(monster);
            }
        }

        private void Remove(int index)
        {
            var last = _monsters.Count - 1;
            _monsters[index] = _monsters[last];
            _monsters.RemoveAt(last);
        }
    }
}

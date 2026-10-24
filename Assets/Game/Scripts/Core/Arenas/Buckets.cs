using System;
using System.Collections.Generic;
using System.Numerics;
using Game.Core.Monsters;

namespace Game.Core.Arenas
{
    public sealed class Buckets
    {
        private readonly float _size;
        private readonly Dictionary<long, List<Monster>> _cells = new Dictionary<long, List<Monster>>();

        public Buckets(float size)
        {
            _size = size;
        }

        public void Clear()
        {
            foreach (var cell in _cells.Values)
            {
                cell.Clear();
            }
        }

        public void Put(Monster monster)
        {
            var position = monster.Position;
            var key = Key(Column(position.X), Column(position.Z));
            if (!_cells.TryGetValue(key, out var cell))
            {
                cell = new List<Monster>();
                _cells.Add(key, cell);
            }
            cell.Add(monster);
        }

        public void Near(Vector3 point, float radius, List<Monster> found)
        {
            found.Clear();
            var limit = radius * radius;
            for (var x = Column(point.X - radius); x <= Column(point.X + radius); x++)
            {
                for (var z = Column(point.Z - radius); z <= Column(point.Z + radius); z++)
                {
                    if (!_cells.TryGetValue(Key(x, z), out var cell))
                    {
                        continue;
                    }
                    foreach (var monster in cell)
                    {
                        var offset = monster.Position - point;
                        if (offset.X * offset.X + offset.Z * offset.Z <= limit)
                        {
                            found.Add(monster);
                        }
                    }
                }
            }
        }

        private int Column(float coordinate) => (int)Math.Floor(coordinate / _size);

        private static long Key(int x, int z) => ((long)x << 32) | (uint)z;
    }
}

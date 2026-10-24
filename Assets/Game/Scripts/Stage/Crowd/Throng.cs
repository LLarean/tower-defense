using System.Collections.Generic;
using Game.Core.Monsters;
using UnityEngine;

namespace Game.Stage.Crowd
{
    public sealed class Throng
    {
        private readonly IReadOnlyDictionary<Breed, Stock> _stocks;
        private readonly Terrain _terrain;
        private readonly Dictionary<Monster, Puppet> _puppets = new Dictionary<Monster, Puppet>();
        private readonly List<Monster> _gone = new List<Monster>();
        private int _frame;

        public Throng(IReadOnlyDictionary<Breed, Stock> stocks, Terrain terrain)
        {
            _stocks = stocks;
            _terrain = terrain;
        }

        public void Sync(IReadOnlyList<Monster> monsters, float seconds)
        {
            _frame++;
            foreach (var monster in monsters)
            {
                var position = Ground(monster.Position.ToUnity());
                if (!_puppets.TryGetValue(monster, out var puppet))
                {
                    var stock = _stocks[monster.Breed];
                    puppet = new Puppet(stock.Take(position, Quaternion.identity), stock);
                    _puppets.Add(monster, puppet);
                }
                puppet.Seen = _frame;
                puppet.Follow(position, seconds);
            }
            Sweep();
        }

        private void Sweep()
        {
            _gone.Clear();
            foreach (var pair in _puppets)
            {
                if (pair.Value.Seen != _frame)
                {
                    _gone.Add(pair.Key);
                }
            }
            foreach (var monster in _gone)
            {
                var puppet = _puppets[monster];
                puppet.Stock.Give(puppet.Body);
                _puppets.Remove(monster);
            }
        }

        private Vector3 Ground(Vector3 point)
        {
            point.y = _terrain.SampleHeight(point) + _terrain.transform.position.y;
            return point;
        }
    }
}

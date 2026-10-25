using System.Collections.Generic;
using Game.Core.Towers;
using Game.Stage.Sound;
using UnityEngine;

namespace Game.Stage.Towers
{
    public sealed class Barrage
    {
        private static readonly Vector3 Lift = new Vector3(0f, 2.5f, 0f);

        private readonly Armory _armory;
        private readonly Transform _root;
        private readonly Acoustics _acoustics;
        private readonly Dictionary<TowerSpec, Stock<Streak>> _stocks = new Dictionary<TowerSpec, Stock<Streak>>();
        private readonly Dictionary<Missile, Streak> _flying = new Dictionary<Missile, Streak>();
        private readonly HashSet<Missile> _seen = new HashSet<Missile>();
        private readonly List<Missile> _gone = new List<Missile>();

        public Barrage(Armory armory, Transform root, Acoustics acoustics)
        {
            _armory = armory;
            _root = root;
            _acoustics = acoustics;
        }

        public void Sync(IReadOnlyList<Missile> missiles)
        {
            _seen.Clear();
            foreach (var missile in missiles)
            {
                var position = missile.Position.ToUnity() + Lift;
                if (!_flying.TryGetValue(missile, out var streak))
                {
                    streak = Stock(missile.Source.Spec).Take(position, Quaternion.identity);
                    streak.Restart();
                    _flying.Add(missile, streak);
                    _acoustics.Fired(missile.Source.Spec);
                }
                streak.Fly(position);
                _seen.Add(missile);
            }
            _gone.Clear();
            foreach (var pair in _flying)
            {
                if (!_seen.Contains(pair.Key))
                {
                    _gone.Add(pair.Key);
                }
            }
            foreach (var missile in _gone)
            {
                Stock(missile.Source.Spec).Give(_flying[missile]);
                _flying.Remove(missile);
            }
        }

        private Stock<Streak> Stock(TowerSpec spec)
        {
            if (!_stocks.TryGetValue(spec, out var stock))
            {
                stock = new Stock<Streak>(_armory.Asset(spec).Missile, _root);
                _stocks.Add(spec, stock);
            }
            return stock;
        }
    }
}

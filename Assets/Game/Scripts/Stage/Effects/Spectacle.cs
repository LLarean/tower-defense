using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Stage.Config;
using Game.Stage.Crowd;
using UnityEngine;

namespace Game.Stage.Effects
{
    public sealed class Spectacle : IFeedback
    {
        private const float Noticeable = 1f;

        private static readonly Vector3 Lift = new Vector3(0f, 1f, 0f);

        private readonly SpectacleAsset _asset;
        private readonly Throng _throng;
        private readonly Tremor _tremor;
        private readonly Transform _root;
        private readonly Dictionary<Burst, Stock<Burst>> _stocks = new Dictionary<Burst, Stock<Burst>>();
        private readonly Stock<Bolt> _bolts;
        private readonly List<(Burst Burst, Stock<Burst> Stock, float Until)> _bursts = new List<(Burst, Stock<Burst>, float)>();
        private readonly List<(Bolt Bolt, float Until)> _strikes = new List<(Bolt, float)>();
        private float _time;

        public Spectacle(SpectacleAsset asset, Throng throng, Tremor tremor, Transform root)
        {
            _asset = asset;
            _throng = throng;
            _tremor = tremor;
            _root = root;
            _bolts = new Stock<Bolt>(asset.Bolt, root);
        }

        public void Wounded(Monster monster, float amount, Element element)
        {
            if (amount >= Noticeable)
            {
                _throng.Flash(monster);
            }
        }

        public void Reacted(ReactionKind kind, System.Numerics.Vector3 position, float radius)
        {
            if (_asset.TryReaction(kind, out var burst, out var shake))
            {
                Play(burst, position.ToUnity() + Lift, radius, burst.Life);
                _tremor.Shake(shake);
            }
        }

        public void Chained(System.Numerics.Vector3 from, System.Numerics.Vector3 to)
        {
            var bolt = _bolts.Take(Vector3.zero, Quaternion.identity);
            bolt.Strike(from.ToUnity() + Lift, to.ToUnity() + Lift);
            _strikes.Add((bolt, _time + _asset.Bolting));
        }

        public void Zoned(ReactionKind kind, System.Numerics.Vector3 position, float radius, float seconds)
        {
            if (_asset.TryReaction(kind, out var burst, out var shake))
            {
                Play(burst, position.ToUnity(), radius, seconds);
                _tremor.Shake(shake);
            }
        }

        public void Died(Monster monster) => Play(_asset.Death, monster.Position.ToUnity() + Lift, 1f, _asset.Death.Life);

        public void Escaped(Monster monster)
        {
        }

        public void Tick(float seconds)
        {
            _time += seconds;
            for (var index = _bursts.Count - 1; index >= 0; index--)
            {
                if (_bursts[index].Until <= _time)
                {
                    _bursts[index].Stock.Give(_bursts[index].Burst);
                    _bursts.RemoveAt(index);
                }
            }
            for (var index = _strikes.Count - 1; index >= 0; index--)
            {
                if (_strikes[index].Until <= _time)
                {
                    _bolts.Give(_strikes[index].Bolt);
                    _strikes.RemoveAt(index);
                }
            }
        }

        private void Play(Burst prefab, Vector3 position, float radius, float seconds)
        {
            if (!_stocks.TryGetValue(prefab, out var stock))
            {
                stock = new Stock<Burst>(prefab, _root);
                _stocks.Add(prefab, stock);
            }
            var burst = stock.Take(position, Quaternion.identity);
            burst.Play(position, radius);
            _bursts.Add((burst, stock, _time + seconds));
        }
    }
}

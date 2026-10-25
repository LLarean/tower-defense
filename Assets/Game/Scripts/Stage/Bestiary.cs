using System.Collections.Generic;
using Game.Core.Monsters;
using Game.Core.Waves;
using Game.Stage.Config;
using Game.Stage.Crowd;
using UnityEngine;

namespace Game.Stage
{
    public sealed class Bestiary
    {
        private readonly Transform _root;
        private readonly Threat _threat;
        private readonly Dictionary<BreedAsset, Breed> _breeds = new Dictionary<BreedAsset, Breed>();
        private readonly Dictionary<Breed, Stock<Body>> _stocks = new Dictionary<Breed, Stock<Body>>();
        private readonly Dictionary<Breed, string> _bosses = new Dictionary<Breed, string>();

        public Bestiary(Transform root, Threat threat)
        {
            _root = root;
            _threat = threat;
        }

        public IReadOnlyDictionary<Breed, Stock<Body>> Stocks => _stocks;

        public bool TryBoss(Breed breed, out string title) => _bosses.TryGetValue(breed, out title);

        public Breed Breed(BreedAsset asset)
        {
            if (!_breeds.TryGetValue(asset, out var breed))
            {
                var retinue = asset.TryFollower(out var follower) ? asset.Retinue(Breed(follower)) : Retinue.None;
                breed = _threat.Toughen(asset.Breed(retinue));
                _breeds.Add(asset, breed);
                _stocks.Add(breed, new Stock<Body>(asset.Model, _root));
                if (asset.Boss)
                {
                    _bosses.Add(breed, asset.Title);
                }
            }
            return breed;
        }
    }
}

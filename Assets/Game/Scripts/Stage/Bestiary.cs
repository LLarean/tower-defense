using System.Collections.Generic;
using Game.Core.Monsters;
using Game.Stage.Config;
using Game.Stage.Crowd;
using UnityEngine;

namespace Game.Stage
{
    public sealed class Bestiary
    {
        private readonly Transform _root;
        private readonly Dictionary<BreedAsset, Breed> _breeds = new Dictionary<BreedAsset, Breed>();
        private readonly Dictionary<Breed, Stock<Body>> _stocks = new Dictionary<Breed, Stock<Body>>();

        public Bestiary(Transform root)
        {
            _root = root;
        }

        public IReadOnlyDictionary<Breed, Stock<Body>> Stocks => _stocks;

        public Breed Breed(BreedAsset asset)
        {
            if (!_breeds.TryGetValue(asset, out var breed))
            {
                breed = asset.Breed();
                _breeds.Add(asset, breed);
                _stocks.Add(breed, new Stock<Body>(asset.Model, _root));
            }
            return breed;
        }
    }
}

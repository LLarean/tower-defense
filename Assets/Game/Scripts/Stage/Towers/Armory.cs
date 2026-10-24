using System.Collections.Generic;
using Game.Core.Towers;
using Game.Stage.Config;

namespace Game.Stage.Towers
{
    public sealed class Armory
    {
        private readonly List<TowerSpec> _specs = new List<TowerSpec>();
        private readonly Dictionary<TowerSpec, TowerAsset> _assets = new Dictionary<TowerSpec, TowerAsset>();

        public Armory(IEnumerable<TowerAsset> assets)
        {
            foreach (var asset in assets)
            {
                var spec = asset.Spec();
                _specs.Add(spec);
                _assets.Add(spec, asset);
            }
        }

        public IReadOnlyList<TowerSpec> Specs => _specs;

        public TowerAsset Asset(TowerSpec spec) => _assets[spec];
    }
}

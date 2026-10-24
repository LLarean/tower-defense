using Game.Stage.Map;
using UnityEngine;

namespace Game.Editor
{
    public sealed class Splatmap
    {
        private readonly Relief _relief;
        private readonly Landscape _landscape;

        public Splatmap(Relief relief, Landscape landscape)
        {
            _relief = relief;
            _landscape = landscape;
        }

        public void Apply(Terrain terrain)
        {
            var data = terrain.terrainData;
            var origin = terrain.transform.position;
            data.terrainLayers = new[] { _landscape.Ground, _landscape.Path, _landscape.Rock };
            var resolution = data.alphamapResolution;
            var alphas = new float[resolution, resolution, 3];
            for (var z = 0; z < resolution; z++)
            {
                for (var x = 0; x < resolution; x++)
                {
                    var point = new Vector2(
                        origin.x + (x + 0.5f) / resolution * data.size.x,
                        origin.z + (z + 0.5f) / resolution * data.size.z);
                    var path = _relief.Road(point);
                    var rock = _relief.Steepness(point) * (1f - path);
                    alphas[z, x, 0] = Mathf.Max(0f, 1f - path - rock);
                    alphas[z, x, 1] = path;
                    alphas[z, x, 2] = rock;
                }
            }
            data.SetAlphamaps(0, 0, alphas);
        }
    }
}

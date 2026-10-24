using UnityEngine;

namespace Game.Editor
{
    public sealed class Heightmap
    {
        private readonly Relief _relief;

        public Heightmap(Relief relief)
        {
            _relief = relief;
        }

        public void Apply(Terrain terrain)
        {
            var data = terrain.terrainData;
            var origin = terrain.transform.position;
            var resolution = data.heightmapResolution;
            var heights = new float[resolution, resolution];
            for (var z = 0; z < resolution; z++)
            {
                for (var x = 0; x < resolution; x++)
                {
                    var point = new Vector2(
                        origin.x + x / (resolution - 1f) * data.size.x,
                        origin.z + z / (resolution - 1f) * data.size.z);
                    heights[z, x] = _relief.Height(point) / data.size.y;
                }
            }
            data.SetHeights(0, 0, heights);
        }
    }
}

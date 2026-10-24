using System.Collections.Generic;
using Game.Stage.Map;
using UnityEngine;

namespace Game.Editor
{
    public sealed class Flora
    {
        private const int DetailResolution = 256;
        private const int Patch = 16;

        private readonly Relief _relief;
        private readonly Corridor _corridor;
        private readonly Landscape _landscape;

        public Flora(Relief relief, Corridor corridor, Landscape landscape)
        {
            _relief = relief;
            _corridor = corridor;
            _landscape = landscape;
        }

        public void Apply(Terrain terrain)
        {
            var random = new System.Random(_landscape.Seed);
            Sow(terrain, random);
            Plant(terrain, random);
        }

        private void Sow(Terrain terrain, System.Random random)
        {
            var data = terrain.terrainData;
            var origin = terrain.transform.position;
            var prototypes = new DetailPrototype[_landscape.Grass.Count];
            for (var index = 0; index < prototypes.Length; index++)
            {
                prototypes[index] = new DetailPrototype
                {
                    prototype = _landscape.Grass[index],
                    usePrototypeMesh = true,
                    renderMode = DetailRenderMode.VertexLit,
                    useInstancing = true,
                    minWidth = 0.8f,
                    maxWidth = 1.4f,
                    minHeight = 0.8f,
                    maxHeight = 1.4f,
                };
            }
            data.detailPrototypes = prototypes;
            data.SetDetailResolution(DetailResolution, Patch);
            for (var layer = 0; layer < prototypes.Length; layer++)
            {
                var counts = new int[DetailResolution, DetailResolution];
                for (var z = 0; z < DetailResolution; z++)
                {
                    for (var x = 0; x < DetailResolution; x++)
                    {
                        var point = new Vector2(
                            origin.x + (x + 0.5f) / DetailResolution * data.size.x,
                            origin.z + (z + 0.5f) / DetailResolution * data.size.z);
                        var lush = Mathf.PerlinNoise(point.x * 0.05f + layer * 31f, point.y * 0.05f) - 0.35f;
                        var bare = _relief.Road(point) > 0.05f || _relief.Steepness(point) > 0.6f;
                        counts[z, x] = bare || lush <= 0f ? 0 : random.Next(0, _landscape.Meadow + 1);
                    }
                }
                data.SetDetailLayer(0, 0, layer, counts);
            }
        }

        private void Plant(Terrain terrain, System.Random random)
        {
            var data = terrain.terrainData;
            var origin = terrain.transform.position;
            var prototypes = new TreePrototype[_landscape.Trees.Count];
            for (var index = 0; index < prototypes.Length; index++)
            {
                prototypes[index] = new TreePrototype { prefab = _landscape.Trees[index] };
            }
            data.treePrototypes = prototypes;
            var trees = new List<TreeInstance>();
            var attempts = _landscape.Forest * 20;
            while (trees.Count < _landscape.Forest && attempts-- > 0 && prototypes.Length > 0)
            {
                var share = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                var point = new Vector2(origin.x + share.x * data.size.x, origin.z + share.y * data.size.z);
                var wild = _relief.Edge(point) < _landscape.Rim * 1.5f || random.NextDouble() < _landscape.Scatter;
                if (!wild || _corridor.Distance(point) < _landscape.Clearance)
                {
                    continue;
                }
                var size = Mathf.Lerp(_landscape.TreeSize.x, _landscape.TreeSize.y, (float)random.NextDouble());
                trees.Add(new TreeInstance
                {
                    prototypeIndex = random.Next(prototypes.Length),
                    position = new Vector3(share.x, data.GetInterpolatedHeight(share.x, share.y) / data.size.y, share.y),
                    rotation = (float)random.NextDouble() * Mathf.PI * 2f,
                    widthScale = size,
                    heightScale = size,
                    color = Color.white,
                    lightmapColor = Color.white,
                });
            }
            data.treeInstances = trees.ToArray();
        }
    }
}

using Game.Stage.Map;
using UnityEngine;

namespace Game.Editor
{
    public sealed class Relief
    {
        private readonly Landscape _landscape;
        private readonly Corridor _corridor;
        private readonly Rect _bounds;

        public Relief(Landscape landscape, Corridor corridor, Rect bounds)
        {
            _landscape = landscape;
            _corridor = corridor;
            _bounds = bounds;
        }

        public float Road(Vector2 point)
        {
            var road = _landscape.Road;
            return 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(road, road + _landscape.Shoulder, _corridor.Distance(point)));
        }

        public float Natural(Vector2 point)
        {
            var seed = _landscape.Seed * 97.13f;
            var scale = _landscape.Scale;
            var bumps = Mathf.PerlinNoise(point.x * scale + seed, point.y * scale + seed) * _landscape.Bumps;
            var edge = Mathf.Min(
                Mathf.Min(point.x - _bounds.xMin, _bounds.xMax - point.x),
                Mathf.Min(point.y - _bounds.yMin, _bounds.yMax - point.y));
            var rim = Mathf.SmoothStep(1f, 0f, Mathf.Clamp01(edge / _landscape.Rim));
            var ridge = 0.6f + 0.4f * Mathf.PerlinNoise(point.x * scale * 3f - seed, point.y * scale * 3f - seed);
            return _landscape.Base + bumps + rim * ridge * _landscape.Hills;
        }

        public float Height(Vector2 point) => Mathf.Lerp(Natural(point), _landscape.Base, Road(point));

        public float Steepness(Vector2 point) => Mathf.Clamp01((Natural(point) - _landscape.Base - _landscape.Bumps) / (_landscape.Hills * 0.4f));
    }
}

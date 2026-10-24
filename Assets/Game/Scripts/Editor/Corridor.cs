using System.Collections.Generic;
using Game.Stage.Map;
using UnityEngine;

namespace Game.Editor
{
    public sealed class Corridor
    {
        private readonly List<Vector2> _starts = new List<Vector2>();
        private readonly List<Vector2> _ends = new List<Vector2>();

        public Corridor(IEnumerable<RoutePath> paths)
        {
            foreach (var path in paths)
            {
                var points = path.transform;
                for (var index = 1; index < points.childCount; index++)
                {
                    _starts.Add(Flat(points.GetChild(index - 1).position));
                    _ends.Add(Flat(points.GetChild(index).position));
                }
            }
        }

        public float Distance(Vector2 point)
        {
            var best = float.MaxValue;
            for (var index = 0; index < _starts.Count; index++)
            {
                var start = _starts[index];
                var segment = _ends[index] - start;
                var share = Mathf.Clamp01(Vector2.Dot(point - start, segment) / segment.sqrMagnitude);
                best = Mathf.Min(best, Vector2.Distance(point, start + segment * share));
            }
            return best;
        }

        private static Vector2 Flat(Vector3 point) => new Vector2(point.x, point.z);
    }
}

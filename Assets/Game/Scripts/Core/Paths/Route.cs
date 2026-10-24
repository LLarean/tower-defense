using System;
using System.Numerics;

namespace Game.Core.Paths
{
    public sealed class Route
    {
        private readonly Vector3[] _points;
        private readonly float[] _marks;

        public Route(params Vector3[] points)
        {
            if (points.Length < 2)
            {
                throw new ArgumentException("A route needs at least two points", nameof(points));
            }
            _points = points;
            _marks = new float[points.Length];
            for (var index = 1; index < points.Length; index++)
            {
                _marks[index] = _marks[index - 1] + Vector3.Distance(points[index - 1], points[index]);
            }
        }

        public float Length => _marks[_marks.Length - 1];

        public Vector3 At(float distance)
        {
            if (distance <= 0f)
            {
                return _points[0];
            }
            if (distance >= Length)
            {
                return _points[_points.Length - 1];
            }
            var index = Array.BinarySearch(_marks, distance);
            if (index >= 0)
            {
                return _points[index];
            }
            var next = ~index;
            var start = _marks[next - 1];
            var share = (distance - start) / (_marks[next] - start);
            return Vector3.Lerp(_points[next - 1], _points[next], share);
        }
    }
}

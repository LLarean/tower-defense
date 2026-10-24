using System;
using System.Numerics;

namespace Game.Core.Towers
{
    public sealed class Plots
    {
        private readonly Vector3 _origin;
        private readonly float _cell;
        private readonly bool[,] _buildable;
        private readonly bool[,] _taken;

        public Plots(Vector3 origin, float cell, bool[,] buildable)
        {
            _origin = origin;
            _cell = cell;
            _buildable = buildable;
            _taken = new bool[buildable.GetLength(0), buildable.GetLength(1)];
        }

        public bool Free(Vector3 point)
        {
            var column = Column(point.X - _origin.X);
            var row = Column(point.Z - _origin.Z);
            return Inside(column, row) && _buildable[column, row] && !_taken[column, row];
        }

        public bool TryClaim(Vector3 point, out Vector3 spot)
        {
            spot = Snap(point);
            if (!Free(point))
            {
                return false;
            }
            _taken[Column(point.X - _origin.X), Column(point.Z - _origin.Z)] = true;
            return true;
        }

        public void Release(Vector3 spot) => _taken[Column(spot.X - _origin.X), Column(spot.Z - _origin.Z)] = false;

        public Vector3 Snap(Vector3 point)
        {
            var x = _origin.X + (Column(point.X - _origin.X) + 0.5f) * _cell;
            var z = _origin.Z + (Column(point.Z - _origin.Z) + 0.5f) * _cell;
            return new Vector3(x, point.Y, z);
        }

        private int Column(float offset) => (int)Math.Floor(offset / _cell);

        private bool Inside(int column, int row) =>
            column >= 0 && row >= 0 && column < _buildable.GetLength(0) && row < _buildable.GetLength(1);
    }
}

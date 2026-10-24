using System.Collections.Generic;
using Game.Core.Paths;
using Game.Core.Towers;
using UnityEngine;

namespace Game.Stage.Map
{
    public sealed class PlotGrid : MonoBehaviour
    {
        [SerializeField] private float _cell = 3f;
        [SerializeField] private int _columns = 30;
        [SerializeField] private int _rows = 30;
        [SerializeField] private float _clearance = 3f;
        [SerializeField] private float _step = 1f;

        public float Cell => _cell;

        public Plots Plots(IReadOnlyList<Route> routes)
        {
            var buildable = new bool[_columns, _rows];
            for (var column = 0; column < _columns; column++)
            {
                for (var row = 0; row < _rows; row++)
                {
                    buildable[column, row] = Clear(Center(column, row), routes);
                }
            }
            return new Plots(transform.position.ToCore(), _cell, buildable);
        }

        private Vector3 Center(int column, int row) =>
            transform.position + new Vector3((column + 0.5f) * _cell, 0f, (row + 0.5f) * _cell);

        private bool Clear(Vector3 center, IReadOnlyList<Route> routes)
        {
            foreach (var route in routes)
            {
                for (var distance = 0f; distance <= route.Length; distance += _step)
                {
                    var point = route.At(distance).ToUnity();
                    var offset = new Vector2(point.x - center.x, point.z - center.z);
                    if (offset.magnitude < _clearance + _cell * 0.5f)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            var size = new Vector3(_cell * 0.9f, 0.1f, _cell * 0.9f);
            for (var column = 0; column < _columns; column++)
            {
                for (var row = 0; row < _rows; row++)
                {
                    Gizmos.DrawCube(Center(column, row), size);
                }
            }
        }
    }
}

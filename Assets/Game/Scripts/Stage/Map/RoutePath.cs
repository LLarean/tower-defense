using Game.Core.Paths;
using UnityEngine;

namespace Game.Stage.Map
{
    public sealed class RoutePath : MonoBehaviour
    {
        [SerializeField] private Color _color = Color.yellow;

        public Route Route()
        {
            var points = new System.Numerics.Vector3[transform.childCount];
            for (var index = 0; index < points.Length; index++)
            {
                points[index] = transform.GetChild(index).position.ToCore();
            }
            return new Route(points);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            for (var index = 1; index < transform.childCount; index++)
            {
                Gizmos.DrawLine(transform.GetChild(index - 1).position, transform.GetChild(index).position);
            }
        }
    }
}

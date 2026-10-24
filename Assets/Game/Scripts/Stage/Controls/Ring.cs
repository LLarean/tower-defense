using UnityEngine;

namespace Game.Stage.Controls
{
    public sealed class Ring : MonoBehaviour
    {
        private const float Lift = 0.3f;

        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _segments = 64;

        public void Show(Vector3 center, float radius)
        {
            _line.positionCount = _segments;
            _line.loop = true;
            for (var index = 0; index < _segments; index++)
            {
                var angle = index * Mathf.PI * 2f / _segments;
                var point = new Vector3(Mathf.Cos(angle) * radius, Lift, Mathf.Sin(angle) * radius);
                _line.SetPosition(index, center + point);
            }
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}

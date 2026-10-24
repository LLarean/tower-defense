using UnityEngine;

namespace Game.Stage.Effects
{
    public sealed class Bolt : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _joints = 8;
        [SerializeField] private float _jitter = 0.6f;

        public void Strike(Vector3 from, Vector3 to)
        {
            _line.positionCount = _joints;
            for (var index = 0; index < _joints; index++)
            {
                var share = index / (_joints - 1f);
                var point = Vector3.Lerp(from, to, share);
                if (index > 0 && index < _joints - 1)
                {
                    point += Random.insideUnitSphere * _jitter;
                }
                _line.SetPosition(index, point);
            }
        }
    }
}

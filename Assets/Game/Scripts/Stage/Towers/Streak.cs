using UnityEngine;

namespace Game.Stage.Towers
{
    public sealed class Streak : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;

        public void Fly(Vector3 position)
        {
            transform.position = position;
        }

        public void Restart()
        {
            _trail.Clear();
        }
    }
}

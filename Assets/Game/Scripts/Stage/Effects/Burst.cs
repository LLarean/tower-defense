using UnityEngine;

namespace Game.Stage.Effects
{
    public sealed class Burst : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _system;
        [SerializeField] private float _life = 2f;
        [SerializeField] private float _span = 1f;

        public float Life => _life;

        public void Play(Vector3 position, float radius)
        {
            transform.position = position;
            transform.localScale = Vector3.one * Mathf.Max(0.5f, radius / _span);
            _system.Clear(true);
            _system.Play(true);
        }
    }
}

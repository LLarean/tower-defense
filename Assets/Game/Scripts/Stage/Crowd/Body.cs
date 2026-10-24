using UnityEngine;

namespace Game.Stage.Crowd
{
    public sealed class Body : MonoBehaviour
    {
        private const float Fade = 8f;

        private static readonly int Emission = Shader.PropertyToID("_EmissionColor");

        [SerializeField] private Renderer[] _renderers = new Renderer[0];
        [SerializeField] private Animator _animator;
        [SerializeField] private float _glow = 1.5f;
        [SerializeField] private float _crown = 2.2f;

        private MaterialPropertyBlock _block;
        private Color _tint = Color.black;
        private Color _shown = Color.clear;
        private float _flash;

        public float Crown => _crown;

        public void Tint(Color tint) => _tint = tint;

        public void Flash() => _flash = 1f;

        public void Pace(float pace)
        {
            if (_animator != null && !Mathf.Approximately(_animator.speed, pace))
            {
                _animator.speed = pace;
            }
        }

        public void Restart()
        {
            _tint = Color.black;
            _flash = 0f;
            _shown = Color.clear;
        }

        public void Tick(float seconds)
        {
            _flash = Mathf.Max(0f, _flash - Fade * seconds);
            var glow = _tint * _glow + Color.white * _flash;
            if (glow == _shown)
            {
                return;
            }
            _shown = glow;
            _block ??= new MaterialPropertyBlock();
            _block.SetColor(Emission, glow);
            foreach (var renderer in _renderers)
            {
                renderer.SetPropertyBlock(_block);
            }
        }
    }
}

using UnityEngine;

namespace Game.Stage.Effects
{
    public sealed class Tremor : MonoBehaviour
    {
        [SerializeField] private float _decay = 6f;
        [SerializeField] private float _limit = 1.2f;

        private float _strength;
        private Vector3 _offset;

        public void Shake(float strength) => _strength = Mathf.Min(_limit, _strength + strength);

        private void LateUpdate()
        {
            transform.localPosition -= _offset;
            _offset = Random.insideUnitSphere * _strength;
            transform.localPosition += _offset;
            _strength = Mathf.MoveTowards(_strength, 0f, _decay * Time.unscaledDeltaTime);
        }
    }
}

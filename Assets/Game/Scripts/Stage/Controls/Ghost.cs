using UnityEngine;

namespace Game.Stage.Controls
{
    public sealed class Ghost : MonoBehaviour
    {
        private const float Lift = 0.15f;

        [SerializeField] private MeshRenderer _marker;
        [SerializeField] private Material _allowed;
        [SerializeField] private Material _denied;
        [SerializeField] private Ring _ring;

        private GameObject _model;
        private float _range;

        public void Show(GameObject prefab, float range, float cell)
        {
            Hide();
            _model = Instantiate(prefab, transform);
            _range = range;
            _marker.transform.localScale = new Vector3(cell, cell, 1f);
            gameObject.SetActive(true);
        }

        public void Place(Vector3 spot, bool allowed)
        {
            transform.position = spot;
            _marker.transform.position = spot + Vector3.up * Lift;
            _marker.sharedMaterial = allowed ? _allowed : _denied;
            _ring.Show(spot, _range);
        }

        public void Hide()
        {
            if (_model != null)
            {
                Destroy(_model);
            }
            _ring.Hide();
            gameObject.SetActive(false);
        }
    }
}

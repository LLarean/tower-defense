using Game.Stage.Config;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Effects
{
    public sealed class Caption
    {
        private const string Hidden = "hidden";

        private readonly Label _label;
        private readonly LetteringData _lettering;
        private Vector3 _anchor;
        private float _age;
        private string _look = Hidden;

        public Caption(VisualElement layer, LetteringData lettering)
        {
            _lettering = lettering;
            _label = new Label { pickingMode = PickingMode.Ignore };
            _label.AddToClassList("caption");
            _label.AddToClassList(Hidden);
            layer.Add(_label);
            _age = lettering.Life;
        }

        public bool Shown => _age < _lettering.Life;

        public void Show(string text, Color color, Vector3 anchor, string look)
        {
            _label.text = text;
            _label.style.color = color;
            _label.RemoveFromClassList(_look);
            _label.AddToClassList(look);
            _label.RemoveFromClassList(Hidden);
            _look = look;
            _anchor = anchor;
            _age = 0f;
        }

        public void Tick(float seconds, Camera camera)
        {
            _age += seconds;
            if (!Shown)
            {
                _label.AddToClassList(Hidden);
                return;
            }
            var progress = _age / _lettering.Life;
            var ahead = camera.WorldToViewportPoint(_anchor).z > 0f;
            var point = RuntimePanelUtils.CameraTransformWorldToPanel(_label.panel, _anchor, camera);
            var lift = 1f - (1f - progress) * (1f - progress);
            var swell = 1f + _lettering.Pop * Mathf.Max(0f, 1f - progress * 4f);
            _label.style.left = point.x;
            _label.style.top = point.y - _lettering.Rise * lift;
            _label.style.opacity = ahead ? 1f - progress * progress * progress : 0f;
            _label.style.scale = new Scale(new Vector3(swell, swell, 1f));
        }
    }
}

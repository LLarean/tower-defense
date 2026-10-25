using Game.Stage.Config;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Effects
{
    public sealed class Captions
    {
        private readonly Caption[] _captions;
        private readonly Camera _camera;
        private int _next;

        public Captions(VisualElement layer, Camera camera, LetteringData lettering)
        {
            _camera = camera;
            _captions = new Caption[lettering.Count];
            for (var index = 0; index < _captions.Length; index++)
            {
                _captions[index] = new Caption(layer, lettering);
            }
        }

        public void Say(string text, Color color, Vector3 anchor, string look)
        {
            if (_captions.Length == 0)
            {
                return;
            }
            _captions[_next].Show(text, color, anchor, look);
            _next = (_next + 1) % _captions.Length;
        }

        public void Tick(float seconds)
        {
            foreach (var caption in _captions)
            {
                if (caption.Shown)
                {
                    caption.Tick(seconds, _camera);
                }
            }
        }
    }
}

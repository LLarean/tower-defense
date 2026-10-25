using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Setting
    {
        private readonly Button _button;
        private readonly string _title;
        private readonly string _key;
        private bool _on;

        public Setting(Button button, string title, string key)
        {
            _button = button;
            _title = title;
            _key = key;
            _on = PlayerPrefs.GetInt(key, 1) == 1;
            button.clicked += Flip;
            Show();
        }

        public bool On => _on;

        private void Flip()
        {
            _on = !_on;
            PlayerPrefs.SetInt(_key, _on ? 1 : 0);
            Show();
        }

        private void Show() => _button.text = _on ? $"{_title}: on" : $"{_title}: off";
    }
}

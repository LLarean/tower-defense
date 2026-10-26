using Game.Stage.Speech;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Menu
{
    public sealed class Fullscreen
    {
        private readonly Button _button;
        private readonly float _window;
        private readonly Lexicon _lexicon;
        private bool _on;

        public Fullscreen(Button button, float window, Lexicon lexicon)
        {
            _button = button;
            _window = window;
            _lexicon = lexicon;
            _on = Screen.fullScreen;
            button.clicked += Flip;
            Show();
        }

        private void Flip()
        {
            _on = !_on;
            var desktop = Display.main;
            if (_on)
                Screen.SetResolution(desktop.systemWidth, desktop.systemHeight, FullScreenMode.FullScreenWindow);
            else
                Screen.SetResolution((int)(desktop.systemWidth * _window), (int)(desktop.systemHeight * _window), FullScreenMode.Windowed);
            Show();
        }

        private void Show() => _button.text = _lexicon.Say(_on ? "{0}: on" : "{0}: off", _lexicon.Say("Fullscreen"));
    }
}

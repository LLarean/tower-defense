using Game.Stage.Speech;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Tempo
    {
        private readonly Button _button;
        private readonly float[] _speeds;
        private readonly Lexicon _lexicon;
        private int _index;
        private bool _halted;

        public Tempo(Button button, float[] speeds, bool online, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _button = button;
            _speeds = speeds;
            button.EnableInClassList("hidden", online || speeds.Length < 2);
            button.clicked += Shift;
            Apply();
        }

        public void Shift()
        {
            _index = (_index + 1) % _speeds.Length;
            Apply();
        }

        public void Halt(bool halted)
        {
            _halted = halted;
            Apply();
        }

        private void Apply()
        {
            Time.timeScale = _halted ? 0f : _speeds[_index];
            _button.text = _lexicon.Say("Speed ×{0}", _speeds[_index].ToString("0.#"));
        }
    }
}

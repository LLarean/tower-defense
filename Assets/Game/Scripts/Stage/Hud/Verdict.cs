using System;
using Game.Core.Battles;
using Game.Stage.Speech;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Verdict : IWidget
    {
        private readonly Battle _battle;
        private readonly VisualElement _screen;
        private readonly Label _outcome;
        private readonly Label _story;
        private readonly (string Victory, string Defeat) _tale;
        private readonly Action _triumph;
        private readonly Lexicon _lexicon;
        private Outcome _shown = Outcome.Ongoing;

        public Verdict(VisualElement root, Battle battle, (string Victory, string Defeat) tale, Action triumph, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _battle = battle;
            _tale = tale;
            _triumph = triumph;
            _screen = root.Q("verdict");
            _outcome = root.Q<Label>("outcome");
            _story = root.Q<Label>("verdict-story");
            _screen.AddToClassList("hidden");
        }

        public void Refresh()
        {
            var outcome = _battle.Outcome;
            if (outcome == _shown)
            {
                return;
            }
            _shown = outcome;
            var won = outcome == Outcome.Won;
            _outcome.text = _lexicon.Say(won ? "Victory" : "Defeat");
            _story.text = _lexicon.Say(won ? _tale.Victory : _tale.Defeat);
            _screen.EnableInClassList("verdict--won", won);
            _screen.EnableInClassList("hidden", outcome == Outcome.Ongoing);
            if (won)
            {
                _triumph();
            }
        }
    }
}

using Game.Core.Battles;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Verdict : IWidget
    {
        private readonly Battle _battle;
        private readonly VisualElement _screen;
        private readonly Label _outcome;
        private Outcome _shown = Outcome.Ongoing;

        public Verdict(VisualElement root, Battle battle, string action, System.Action act)
        {
            _battle = battle;
            _screen = root.Q("verdict");
            _outcome = root.Q<Label>("outcome");
            var again = root.Q<Button>("again");
            again.text = action;
            again.clicked += act;
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
            _outcome.text = outcome == Outcome.Won ? "Victory" : "Defeat";
            _screen.EnableInClassList("hidden", outcome == Outcome.Ongoing);
        }
    }
}

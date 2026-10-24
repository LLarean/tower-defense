using Game.Core.Battles;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Verdict : IWidget
    {
        private readonly Battle _battle;
        private readonly VisualElement _screen;
        private readonly Label _outcome;
        private Outcome _shown = Outcome.Ongoing;

        public Verdict(VisualElement root, Battle battle)
        {
            _battle = battle;
            _screen = root.Q("verdict");
            _outcome = root.Q<Label>("outcome");
            root.Q<Button>("again").clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

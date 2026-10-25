using Game.Core.Battles;
using Game.Stage.Config;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Briefing : IWidget
    {
        private readonly Battle _battle;
        private readonly VisualElement _screen;

        public Briefing(VisualElement root, Battle battle, LevelAsset level, string difficulty)
        {
            _battle = battle;
            _screen = root.Q("briefing");
            root.Q<Label>("briefing-title").text = level.Title;
            root.Q<Label>("briefing-text").text = level.Briefing;
            root.Q<Label>("briefing-difficulty").text = $"Difficulty: {difficulty}";
            root.Q<Button>("briefing-go").clicked += Dismiss;
            _screen.EnableInClassList("hidden", string.IsNullOrEmpty(level.Briefing));
        }

        public void Refresh()
        {
            if (_battle.Started)
            {
                Dismiss();
            }
        }

        private void Dismiss() => _screen.AddToClassList("hidden");
    }
}

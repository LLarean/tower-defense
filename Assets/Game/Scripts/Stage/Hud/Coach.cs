using Game.Core.Battles;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Coach : IWidget
    {
        private const string Build = "Pick a tower below or press 1–7, then click near the road to build it.";
        private const string Begin = "Build a few more or press Start (Space) when you are ready.";

        private readonly Battle _battle;
        private readonly Label _hint;
        private string _shown = string.Empty;

        public Coach(VisualElement root, Battle battle)
        {
            _battle = battle;
            _hint = root.Q<Label>("hint");
        }

        public void Refresh()
        {
            var advice = _battle.Started ? string.Empty : _battle.Garrison.Towers.Count == 0 ? Build : Begin;
            if (advice == _shown)
            {
                return;
            }
            _shown = advice;
            _hint.text = advice;
            _hint.EnableInClassList("hidden", advice.Length == 0);
        }
    }
}

using Game.Stage.Config;
using Game.Stage.Speech;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Codex
    {
        private readonly VisualElement _screen;

        public Codex(VisualElement root, Recipes recipes, LoreAsset lore, Lexicon lexicon)
        {
            _screen = root.Q("codex-screen");
            var list = root.Q<ScrollView>("codex-list");
            foreach (var (first, second, kind) in recipes.All)
            {
                var row = new VisualElement();
                row.AddToClassList("codex-row");
                var name = new Label($"{first} + {second} → {lexicon.Say(lore.Title(kind))}");
                name.AddToClassList("codex-name");
                var text = new Label(lexicon.Say(lore.Text(kind)));
                text.AddToClassList("codex-text");
                row.Add(name);
                row.Add(text);
                list.Add(row);
            }
            root.Q<Button>("codex-close").clicked += Close;
            Close();
        }

        public void Open() => _screen.RemoveFromClassList("hidden");

        public void Close() => _screen.AddToClassList("hidden");
    }
}

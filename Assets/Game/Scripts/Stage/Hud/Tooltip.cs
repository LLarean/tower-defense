using System.Text;
using Game.Core.Monsters;
using Game.Core.Towers;
using Game.Stage.Towers;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Tooltip
    {
        private readonly Recipes _recipes;
        private readonly Glossary _glossary;
        private readonly Label _label;
        private readonly StringBuilder _text = new StringBuilder();

        public Tooltip(VisualElement root, Armory armory, Recipes recipes, Glossary glossary)
        {
            _recipes = recipes;
            _glossary = glossary;
            _label = root.Q<Label>("tooltip");
            var slots = root.Query<Button>(className: "slot").ToList();
            for (var index = 0; index < slots.Count && index < armory.Specs.Count; index++)
            {
                var spec = armory.Specs[index];
                var title = armory.Asset(spec).Title;
                slots[index].RegisterCallback<PointerEnterEvent>(_ => Show(title, spec));
                slots[index].RegisterCallback<PointerLeaveEvent>(_ => _label.AddToClassList("hidden"));
            }
            _label.AddToClassList("hidden");
        }

        private void Show(string title, TowerSpec spec)
        {
            var grade = spec.Grade(0);
            _text.Clear();
            _text.Append($"<size=120%><b>{title}</b></size>   <color=#FFCD64>{spec.Price} gold</color>\n");
            _text.Append($"Damage <b>{grade.Damage:0}</b>  ·  Range <b>{grade.Range:0}</b>  ·  Reload <b>{grade.Cooldown:0.0}s</b>\n");
            foreach (var element in spec.Elements)
            {
                var mark = Statuses.MarkOf(element);
                _text.Append($"{_glossary.Condition(mark)}: {_glossary.Effect(mark)}\n");
                _text.Append("<color=#A0A8B8>Reacts with:</color>\n");
                foreach (var (partner, kind) in _recipes.With(element))
                {
                    _text.Append($"   + {partner}  →  {_glossary.Reaction(kind)}\n");
                }
            }
            _label.text = _text.ToString().TrimEnd();
            _label.RemoveFromClassList("hidden");
        }
    }
}

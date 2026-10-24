using System.Text;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Towers;
using Game.Stage.Config;
using Game.Stage.Towers;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Tooltip
    {
        private readonly Recipes _recipes;
        private readonly LoreAsset _lore;
        private readonly Label _label;
        private readonly StringBuilder _text = new StringBuilder();

        public Tooltip(VisualElement root, Armory armory, Recipes recipes, LoreAsset lore)
        {
            _recipes = recipes;
            _lore = lore;
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
            _text.Append($"<b>{title}</b>  {spec.Price} gold\n");
            _text.Append($"Damage {grade.Damage:0} · Range {grade.Range:0} · Reload {grade.Cooldown:0.0}s\n");
            foreach (var element in spec.Elements)
            {
                var mark = Statuses.MarkOf(element);
                _text.Append($"{_lore.Title(mark)}: {_lore.Effect(mark)}\n");
                foreach (var (partner, kind) in _recipes.With(element))
                {
                    _text.Append($"  + {partner} → {_lore.Title(kind)}\n");
                }
            }
            _label.text = _text.ToString().TrimEnd();
            _label.RemoveFromClassList("hidden");
        }
    }
}

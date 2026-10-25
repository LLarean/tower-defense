using System.Text;
using Game.Core.Monsters;
using Game.Core.Towers;
using Game.Stage.Speech;
using Game.Stage.Towers;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Tooltip
    {
        private readonly Recipes _recipes;
        private readonly Glossary _glossary;
        private readonly Lexicon _lexicon;
        private readonly Label _label;
        private readonly StringBuilder _text = new StringBuilder();

        public Tooltip(VisualElement root, Armory armory, Recipes recipes, Glossary glossary, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _recipes = recipes;
            _glossary = glossary;
            _label = root.Q<Label>("tooltip");
            var slots = root.Query<Button>(className: "slot").ToList();
            for (var index = 0; index < slots.Count && index < armory.Specs.Count; index++)
            {
                var spec = armory.Specs[index];
                var title = lexicon.Say(armory.Asset(spec).Title);
                slots[index].RegisterCallback<PointerEnterEvent>(_ => Show(title, spec));
                slots[index].RegisterCallback<PointerLeaveEvent>(_ => _label.AddToClassList("hidden"));
            }
            _label.AddToClassList("hidden");
        }

        private void Show(string title, TowerSpec spec)
        {
            var grade = spec.Grade(0);
            _text.Clear();
            _text.Append($"<size=120%><b>{title}</b></size>   <color=#FFCD64>").Append(_lexicon.Say("{0} gold", spec.Price)).Append("</color>\n");
            _text.Append(_lexicon.Say("Damage")).Append($" <b>{grade.Damage:0}</b>  ·  ").Append(_lexicon.Say("Range")).Append($" <b>{grade.Range:0}</b>  ·  ").Append(_lexicon.Say("Reload")).Append(" <b>").Append(_lexicon.Say("{0}s", grade.Cooldown.ToString("0.0"))).Append("</b>\n");
            foreach (var element in spec.Elements)
            {
                var mark = Statuses.MarkOf(element);
                _text.Append($"{_glossary.Condition(mark)}: {_glossary.Effect(mark)}\n");
                _text.Append("<color=#A0A8B8>").Append(_lexicon.Say("Reacts with:")).Append("</color>\n");
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

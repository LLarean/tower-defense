using System.Text;
using Game.Core.Monsters;
using Game.Core.Towers;
using Game.Stage.Controls;
using Game.Stage.Orders;
using Game.Stage.Towers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Inspector : IWidget
    {
        private const float Period = 0.1f;

        private readonly Pick _pick;
        private readonly Armory _armory;
        private readonly Glossary _glossary;
        private readonly VisualElement _panel;
        private readonly VisualElement _actions;
        private readonly Label _title;
        private readonly Label _stats;
        private readonly Button _upgrade;
        private readonly Button _sell;
        private readonly StringBuilder _text = new StringBuilder();
        private Tower _shown;
        private int _shownLevel = -1;
        private float _next;

        public Inspector(VisualElement root, Pick pick, Armory armory, IOrders orders, Glossary glossary)
        {
            _pick = pick;
            _armory = armory;
            _glossary = glossary;
            _panel = root.Q("inspector");
            _actions = _panel.Q(className: "actions");
            _title = root.Q<Label>("title");
            _stats = root.Q<Label>("stats");
            _upgrade = root.Q<Button>("upgrade");
            _sell = root.Q<Button>("sell");
            _upgrade.clicked += () => Apply(orders.Upgrade);
            _sell.clicked += () => Apply(orders.Sell);
        }

        public void Refresh()
        {
            if (_pick.TrySelected(out var tower))
            {
                Show(tower);
            }
            else if (_pick.TryMonster(out var monster))
            {
                Show(monster);
            }
            else
            {
                _shown = null;
                _panel.AddToClassList("hidden");
            }
        }

        private void Show(Tower tower)
        {
            _panel.RemoveFromClassList("hidden");
            _actions.RemoveFromClassList("hidden");
            if (tower == _shown && tower.Level == _shownLevel)
            {
                return;
            }
            _shown = tower;
            _shownLevel = tower.Level;
            var grade = tower.Grade;
            var next = tower.Upgradable ? tower.Spec.Grade(tower.Level + 1) : grade;
            _title.text = $"{_armory.Asset(tower.Spec).Title} · level {tower.Level + 1}/{tower.Spec.Levels}";
            _text.Clear();
            foreach (var element in tower.Spec.Elements)
            {
                _text.Append(_glossary.Element(element)).Append("  ");
            }
            _text.AppendLine();
            Row("Damage", grade.Damage, next.Damage, "0");
            Row("Range", grade.Range, next.Range, "0");
            Row("Reload", grade.Cooldown, next.Cooldown, "0.0");
            foreach (var element in tower.Spec.Elements)
            {
                Row(_glossary.Condition(Statuses.MarkOf(element)), grade.Potency, next.Potency, "0.##");
            }
            _stats.text = _text.ToString().TrimEnd();
            _upgrade.text = tower.Upgradable ? $"Upgrade {tower.UpgradeCost}" : "Max level";
            _upgrade.SetEnabled(tower.Upgradable);
            _sell.text = $"Sell {tower.Refund}";
        }

        private void Show(Monster monster)
        {
            _shown = null;
            _panel.RemoveFromClassList("hidden");
            _actions.AddToClassList("hidden");
            if (Time.unscaledTime < _next)
            {
                return;
            }
            _next = Time.unscaledTime + Period;
            var breed = monster.Breed;
            _title.text = breed.Name;
            _text.Clear();
            _text.Append($"Health <b>{monster.Health:0}</b> / {breed.Health:0}\nArmor {breed.Armor * 100f:0}%  ·  Speed {breed.Speed * monster.Pace:0.0}");
            foreach (var condition in Statuses.All)
            {
                if (monster.Has(condition))
                {
                    _text.Append('\n').Append(_glossary.Condition(condition)).Append($"  {monster.Remaining(condition):0.0}s");
                }
            }
            _stats.text = _text.ToString();
        }

        private void Row(string name, float now, float next, string format)
        {
            _text.Append(name).Append("  <b>").Append(now.ToString(format)).Append("</b>");
            if (!Mathf.Approximately(now, next))
            {
                _text.Append("  <color=#8CDC8C>→ ").Append(next.ToString(format)).Append("</color>");
            }
            _text.AppendLine();
        }

        private void Apply(System.Action<Tower> order)
        {
            if (_pick.TrySelected(out var tower))
            {
                order(tower);
            }
        }
    }
}

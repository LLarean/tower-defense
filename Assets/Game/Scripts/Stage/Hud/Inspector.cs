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

        public Inspector(VisualElement root, Pick pick, Armory armory, IOrders orders)
        {
            _pick = pick;
            _armory = armory;
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
            _title.text = $"{_armory.Asset(tower.Spec).Title} — level {tower.Level + 1}";
            _text.Clear();
            _text.Append("Elements ").AppendJoin(" + ", tower.Spec.Elements).AppendLine();
            _text.Append($"Damage {grade.Damage:0}\nRange {grade.Range:0}\nReload {grade.Cooldown:0.0}s\nStatus power {grade.Potency:0.##} for {grade.Duration:0.#}s");
            _stats.text = _text.ToString();
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
            _text.Append($"Health {monster.Health:0}/{breed.Health:0}\nArmor {breed.Armor * 100f:0}%\nSpeed {breed.Speed * monster.Pace:0.0}");
            foreach (var condition in Statuses.All)
            {
                if (monster.Has(condition))
                {
                    _text.Append($"\n{condition} {monster.Remaining(condition):0.0}s");
                }
            }
            _stats.text = _text.ToString();
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

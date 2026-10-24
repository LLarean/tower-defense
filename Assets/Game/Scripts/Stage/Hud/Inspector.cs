using Game.Core.Towers;
using Game.Stage.Controls;
using Game.Stage.Orders;
using Game.Stage.Towers;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Inspector : IWidget
    {
        private readonly Pick _pick;
        private readonly Armory _armory;
        private readonly VisualElement _panel;
        private readonly Label _title;
        private readonly Label _stats;
        private readonly Button _upgrade;
        private readonly Button _sell;
        private Tower _shown;
        private int _shownLevel = -1;

        public Inspector(VisualElement root, Pick pick, Armory armory, IOrders orders)
        {
            _pick = pick;
            _armory = armory;
            _panel = root.Q("inspector");
            _title = root.Q<Label>("title");
            _stats = root.Q<Label>("stats");
            _upgrade = root.Q<Button>("upgrade");
            _sell = root.Q<Button>("sell");
            _upgrade.clicked += () => Apply(orders.Upgrade);
            _sell.clicked += () => Apply(orders.Sell);
        }

        public void Refresh()
        {
            var selected = _pick.TrySelected(out var tower);
            _panel.EnableInClassList("hidden", !selected);
            if (!selected || (tower == _shown && tower.Level == _shownLevel))
            {
                return;
            }
            _shown = tower;
            _shownLevel = tower.Level;
            var grade = tower.Grade;
            _title.text = $"{_armory.Asset(tower.Spec).Title} — level {tower.Level + 1}";
            _stats.text = $"Damage {grade.Damage:0}\nRange {grade.Range:0}\nReload {grade.Cooldown:0.0}s";
            _upgrade.text = tower.Upgradable ? $"Upgrade {tower.UpgradeCost}" : "Max level";
            _upgrade.SetEnabled(tower.Upgradable);
            _sell.text = $"Sell {tower.Refund}";
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

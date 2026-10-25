using System.Collections.Generic;
using Game.Core.Economy;
using Game.Core.Towers;
using Game.Stage.Config;
using Game.Stage.Controls;
using Game.Stage.Towers;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Arsenal : IWidget
    {
        private readonly Armory _armory;
        private readonly Wallet _wallet;
        private readonly List<Button> _slots = new List<Button>();
        private int _shownGold = -1;

        public Arsenal(VisualElement root, Armory armory, Wallet wallet, Hand hand)
        {
            _armory = armory;
            _wallet = wallet;
            var bar = root.Q("arsenal");
            for (var index = 0; index < armory.Specs.Count; index++)
            {
                var spec = armory.Specs[index];
                var slot = Slot(armory.Asset(spec), spec, index + 1);
                slot.clicked += () => hand.Choose(spec);
                bar.Add(slot);
                _slots.Add(slot);
            }
        }

        public void Refresh()
        {
            if (_wallet.Gold == _shownGold)
            {
                return;
            }
            _shownGold = _wallet.Gold;
            for (var index = 0; index < _slots.Count; index++)
            {
                _slots[index].EnableInClassList("slot--poor", _armory.Specs[index].Price > _shownGold);
            }
        }

        private static Button Slot(TowerAsset asset, TowerSpec spec, int key)
        {
            var slot = new Button();
            slot.AddToClassList("slot");
            slot.AddToClassList($"slot--{spec.Elements[0].ToString().ToLowerInvariant()}");
            var icon = new VisualElement { pickingMode = PickingMode.Ignore };
            icon.AddToClassList("slot-icon");
            icon.style.backgroundImage = new StyleBackground(asset.Icon);
            slot.Add(icon);
            slot.Add(Caption($"{key}", "slot-key"));
            slot.Add(Caption($"{spec.Price}", "slot-price"));
            return slot;
        }

        private static Label Caption(string text, string look)
        {
            var label = new Label(text) { pickingMode = PickingMode.Ignore };
            label.AddToClassList(look);
            return label;
        }
    }
}

using System.Collections.Generic;
using Game.Core.Economy;
using Game.Core.Towers;
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
                var slot = new Button(() => hand.Choose(spec)) { text = $"[{index + 1}] {armory.Asset(spec).Title}\n{spec.Price} gold" };
                slot.AddToClassList("slot");
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
    }
}

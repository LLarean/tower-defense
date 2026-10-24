using Game.Stage.Hud;
using Game.Stage.Orders;
using Game.Stage.Towers;
using UnityEngine.InputSystem;

namespace Game.Stage.Controls
{
    public sealed class Hotkeys
    {
        private static readonly Key[] Slots = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9 };

        private readonly Armory _armory;
        private readonly IOrders _orders;
        private readonly Hand _hand;
        private readonly Pause _pause;

        public Hotkeys(Armory armory, IOrders orders, Hand hand, Pause pause)
        {
            _armory = armory;
            _orders = orders;
            _hand = hand;
            _pause = pause;
        }

        public void Tick()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }
            for (var slot = 0; slot < Slots.Length && slot < _armory.Specs.Count; slot++)
            {
                if (keyboard[Slots[slot]].wasPressedThisFrame)
                {
                    _hand.Choose(_armory.Specs[slot]);
                }
            }
            if (keyboard.escapeKey.wasPressedThisFrame && !_hand.TryCancel())
            {
                _pause.Toggle();
            }
            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                _orders.Summon();
            }
            if (!_hand.TrySelected(out var tower))
            {
                return;
            }
            if (keyboard.uKey.wasPressedThisFrame)
            {
                _orders.Upgrade(tower);
            }
            if (keyboard.xKey.wasPressedThisFrame)
            {
                _orders.Sell(tower);
            }
        }
    }
}

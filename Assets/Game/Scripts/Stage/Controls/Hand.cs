using Game.Stage.Orders;
using Game.Stage.Towers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Stage.Controls
{
    public sealed class Hand
    {
        private const float Reach = 500f;

        private static readonly Key[] Slots = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9 };

        private readonly Camera _camera;
        private readonly Armory _armory;
        private readonly IOrders _orders;
        private readonly Blueprint _blueprint;
        private readonly Pick _pick;

        public Hand(Camera camera, Armory armory, IOrders orders, Blueprint blueprint, Pick pick)
        {
            _camera = camera;
            _armory = armory;
            _orders = orders;
            _blueprint = blueprint;
            _pick = pick;
        }

        public void Tick()
        {
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            if (keyboard == null || mouse == null)
            {
                return;
            }
            for (var slot = 0; slot < Slots.Length && slot < _armory.Specs.Count; slot++)
            {
                if (keyboard[Slots[slot]].wasPressedThisFrame)
                {
                    _pick.Clear();
                    _blueprint.Choose(_armory.Specs[slot]);
                }
            }
            if (keyboard.escapeKey.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
            {
                _blueprint.Cancel();
                _pick.Clear();
            }
            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                _orders.Summon();
            }
            Command(keyboard);
            Point(mouse);
            _pick.Refresh();
        }

        private void Command(Keyboard keyboard)
        {
            if (!_pick.TrySelected(out var tower))
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

        private void Point(Mouse mouse)
        {
            var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit, Reach))
            {
                return;
            }
            var click = mouse.leftButton.wasPressedThisFrame;
            if (_blueprint.Active)
            {
                _blueprint.Aim(hit.point, click);
            }
            else if (click)
            {
                _pick.Choose(hit.point);
            }
        }
    }
}

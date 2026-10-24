using Game.Core.Towers;
using Game.Stage.Hud;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Stage.Controls
{
    public sealed class Hand
    {
        private const float Reach = 500f;

        private readonly Camera _camera;
        private readonly ICover _cover;
        private readonly Blueprint _blueprint;
        private readonly Pick _pick;

        public Hand(Camera camera, ICover cover, Blueprint blueprint, Pick pick)
        {
            _camera = camera;
            _cover = cover;
            _blueprint = blueprint;
            _pick = pick;
        }

        public void Choose(TowerSpec spec)
        {
            _pick.Clear();
            _blueprint.Choose(spec);
        }

        public void Cancel()
        {
            _blueprint.Cancel();
            _pick.Clear();
        }

        public void Tick()
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }
            if (mouse.rightButton.wasPressedThisFrame)
            {
                Cancel();
            }
            var pointer = mouse.position.ReadValue();
            if (!_cover.Covers(pointer))
            {
                Point(pointer, mouse.leftButton.wasPressedThisFrame);
            }
            _pick.Refresh();
        }

        private void Point(Vector2 pointer, bool click)
        {
            if (!Physics.Raycast(_camera.ScreenPointToRay(pointer), out var hit, Reach))
            {
                return;
            }
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

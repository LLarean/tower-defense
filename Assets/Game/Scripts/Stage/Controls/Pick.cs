using Game.Core.Towers;
using UnityEngine;

namespace Game.Stage.Controls
{
    public sealed class Pick
    {
        private readonly Garrison _garrison;
        private readonly Ring _ring;
        private readonly float _reach;
        private Tower _tower;

        public Pick(Garrison garrison, Ring ring, float reach)
        {
            _garrison = garrison;
            _ring = ring;
            _reach = reach;
        }

        public bool TrySelected(out Tower tower)
        {
            tower = _tower;
            return _tower != null && Contains(_tower);
        }

        public void Choose(Vector3 point)
        {
            _tower = null;
            var best = _reach;
            foreach (var tower in _garrison.Towers)
            {
                var distance = Vector3.Distance(tower.Position.ToUnity(), point);
                if (distance < best)
                {
                    best = distance;
                    _tower = tower;
                }
            }
            Refresh();
        }

        public void Clear()
        {
            _tower = null;
            Refresh();
        }

        public void Refresh()
        {
            if (TrySelected(out var tower))
            {
                _ring.Show(tower.Position.ToUnity(), tower.Grade.Range);
                return;
            }
            _ring.Hide();
        }

        private bool Contains(Tower tower)
        {
            foreach (var standing in _garrison.Towers)
            {
                if (standing == tower)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

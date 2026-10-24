using Game.Core.Arenas;
using Game.Core.Monsters;
using Game.Core.Towers;
using UnityEngine;

namespace Game.Stage.Controls
{
    public sealed class Pick
    {
        private const float Grasp = 2f;
        private const float Halo = 1.3f;

        private readonly Garrison _garrison;
        private readonly Horde _horde;
        private readonly Ring _ring;
        private readonly float _reach;
        private Tower _tower;
        private Monster _monster;

        public Pick(Garrison garrison, Horde horde, Ring ring, float reach)
        {
            _garrison = garrison;
            _horde = horde;
            _ring = ring;
            _reach = reach;
        }

        public bool TrySelected(out Tower tower)
        {
            tower = _tower;
            return _tower != null && Standing(_tower);
        }

        public bool TryMonster(out Monster monster)
        {
            monster = _monster;
            return _monster != null && _monster.Alive && Walking(_monster);
        }

        public void Choose(Vector3 point)
        {
            Clear();
            var best = Grasp;
            foreach (var monster in _horde.Monsters)
            {
                var distance = Flat(monster.Position.ToUnity(), point);
                if (distance < best)
                {
                    best = distance;
                    _monster = monster;
                }
            }
            if (_monster == null)
            {
                best = _reach;
                foreach (var tower in _garrison.Towers)
                {
                    var distance = Flat(tower.Position.ToUnity(), point);
                    if (distance < best)
                    {
                        best = distance;
                        _tower = tower;
                    }
                }
            }
            Refresh();
        }

        public void Clear()
        {
            _tower = null;
            _monster = null;
            _ring.Hide();
        }

        public void Refresh()
        {
            if (TrySelected(out var tower))
            {
                _ring.Show(tower.Position.ToUnity(), tower.Grade.Range);
            }
            else if (TryMonster(out var monster))
            {
                _ring.Show(monster.Position.ToUnity(), Halo);
            }
            else
            {
                _ring.Hide();
            }
        }

        private static float Flat(Vector3 first, Vector3 second) => Vector2.Distance(new Vector2(first.x, first.z), new Vector2(second.x, second.z));

        private bool Standing(Tower tower)
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

        private bool Walking(Monster monster)
        {
            foreach (var walking in _horde.Monsters)
            {
                if (walking == monster)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

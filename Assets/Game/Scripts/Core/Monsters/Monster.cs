using System;
using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Paths;
using Game.Core.Reactions;

namespace Game.Core.Monsters
{
    public sealed class Monster
    {
        private readonly Breed _breed;
        private readonly Route _route;
        private readonly Statuses _statuses = new Statuses();
        private float _health;
        private float _distance;

        public Monster(Breed breed, Route route)
        {
            _breed = breed;
            _route = route;
            _health = breed.Health;
        }

        public Breed Breed => _breed;

        public Vector3 Position => _route.At(_distance);

        public float Distance => _distance;

        public float Vitality => _health / _breed.Health;

        public bool Alive => _health > 0f;

        public bool Escaped => _distance >= _route.Length;

        public bool Has(Condition condition) => _statuses.Has(condition);

        public void Take(Hit hit, Arena arena)
        {
            if (!Alive || _breed.Resists(hit.Element))
            {
                return;
            }
            Wound(hit.Damage, hit.Element, arena);
            foreach (var condition in Statuses.All)
            {
                if (_statuses.Has(condition) && arena.Chemistry.TryFind(condition, hit.Element, out IReaction reaction))
                {
                    _statuses.Cure(condition);
                    reaction.Trigger(this, hit, arena);
                    return;
                }
            }
            _statuses.Inflict(Statuses.MarkOf(hit.Element), hit.Potency, hit.Duration);
        }

        public void Wound(float damage, Element element, Arena arena)
        {
            if (!Alive || _breed.Resists(element))
            {
                return;
            }
            var armor = Math.Max(0f, _breed.Armor - _statuses.Potency(Condition.Corroded));
            var amount = damage * (1f - armor) * (1f + _statuses.Potency(Condition.Sundered));
            _health -= amount;
            arena.Feedback.Wounded(this, amount, element);
        }

        public void Inflict(Condition condition, float potency, float duration) => _statuses.Inflict(condition, potency, duration);

        public void Cure(Condition condition) => _statuses.Cure(condition);

        public void Push(float meters) => _distance = Math.Max(0f, _distance - meters);

        public void Tick(float seconds, Arena arena)
        {
            var burn = _statuses.Potency(Condition.Burning);
            if (burn > 0f)
            {
                Wound(burn * seconds, Element.Fire, arena);
            }
            var poison = _statuses.Potency(Condition.Poisoned);
            if (poison > 0f)
            {
                Wound(poison * seconds, Element.Poison, arena);
            }
            _distance += _breed.Speed * _statuses.Pace * seconds;
            _statuses.Tick(seconds);
        }
    }
}

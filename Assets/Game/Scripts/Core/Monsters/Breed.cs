using System;
using Game.Core.Elements;

namespace Game.Core.Monsters
{
    public sealed class Breed
    {
        private readonly Element[] _immunities;

        public Breed(string name, float health, float armor, float speed, int bounty, int harm, params Element[] immunities)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Speed = speed;
            Bounty = bounty;
            Harm = harm;
            _immunities = immunities;
        }

        public string Name { get; }

        public float Health { get; }

        public float Armor { get; }

        public float Speed { get; }

        public int Bounty { get; }

        public int Harm { get; }

        public bool Resists(Element element) => Array.IndexOf(_immunities, element) >= 0;
    }
}

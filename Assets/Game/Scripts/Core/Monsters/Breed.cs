using System;
using Game.Core.Elements;

namespace Game.Core.Monsters
{
    public sealed class Breed
    {
        private readonly Element[] _immunities;

        public Breed(string name, float health, float armor, float speed, int bounty, int harm, params Element[] immunities)
            : this(name, health, armor, speed, bounty, harm, immunities, Retinue.None)
        {
        }

        private Breed(string name, float health, float armor, float speed, int bounty, int harm, Element[] immunities, Retinue retinue)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Speed = speed;
            Bounty = bounty;
            Harm = harm;
            _immunities = immunities;
            Retinue = retinue;
        }

        public string Name { get; }

        public float Health { get; }

        public float Armor { get; }

        public float Speed { get; }

        public int Bounty { get; }

        public int Harm { get; }

        public Retinue Retinue { get; }

        public Breed Scaled(float health, float bounty) =>
            new Breed(Name, Health * health, Armor, Speed, (int)Math.Round(Bounty * bounty), Harm, _immunities, Retinue);

        public Breed Leading(Retinue retinue) => new Breed(Name, Health, Armor, Speed, Bounty, Harm, _immunities, retinue);

        public bool Resists(Element element) => Array.IndexOf(_immunities, element) >= 0;
    }
}

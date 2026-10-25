using Game.Core.Monsters;

namespace Game.Core.Waves
{
    public sealed class Threat
    {
        private readonly float _health;
        private readonly float _bounty;
        private readonly float _crowding;
        private readonly int _players;

        public Threat(float health, float bounty, float crowding, int players)
        {
            _health = health;
            _bounty = bounty;
            _crowding = crowding;
            _players = players;
        }

        public float Vigor => _health * Crowd;

        public float Wealth => _bounty * Crowd;

        private float Crowd => 1f + _crowding * (_players - 1);

        public Breed Toughen(Breed breed) => breed.Scaled(Vigor, Wealth);
    }
}

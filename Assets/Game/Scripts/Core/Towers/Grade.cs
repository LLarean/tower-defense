namespace Game.Core.Towers
{
    public sealed class Grade
    {
        public Grade(float damage, float potency, float duration, float range, float cooldown, int cost)
        {
            Damage = damage;
            Potency = potency;
            Duration = duration;
            Range = range;
            Cooldown = cooldown;
            Cost = cost;
        }

        public float Damage { get; }

        public float Potency { get; }

        public float Duration { get; }

        public float Range { get; }

        public float Cooldown { get; }

        public int Cost { get; }
    }
}

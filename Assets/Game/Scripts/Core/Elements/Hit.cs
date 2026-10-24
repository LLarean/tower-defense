namespace Game.Core.Elements
{
    public readonly struct Hit
    {
        public Hit(Element element, float damage, float potency, float duration)
        {
            Element = element;
            Damage = damage;
            Potency = potency;
            Duration = duration;
        }

        public Element Element { get; }

        public float Damage { get; }

        public float Potency { get; }

        public float Duration { get; }
    }
}

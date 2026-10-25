namespace Game.Core.Monsters
{
    public sealed class Retinue
    {
        public static readonly Retinue None = new Retinue(new Breed("None", 1f, 0f, 0f, 0, 0), 0, 0, 0f);

        public Retinue(Breed follower, int count, int calls, float spacing)
        {
            Follower = follower;
            Count = count;
            Calls = calls;
            Spacing = spacing;
        }

        public Breed Follower { get; }

        public int Count { get; }

        public int Calls { get; }

        public float Spacing { get; }

        public float Threshold(int call) => 1f - (call + 1f) / (Calls + 1f);
    }
}

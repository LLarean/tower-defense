using Game.Core.Monsters;

namespace Game.Core.Waves
{
    public sealed class Squad
    {
        public Squad(Breed breed, int count, float interval, float delay, int route)
        {
            Breed = breed;
            Count = count;
            Interval = interval;
            Delay = delay;
            Route = route;
        }

        public Breed Breed { get; }

        public int Count { get; }

        public float Interval { get; }

        public float Delay { get; }

        public int Route { get; }
    }
}

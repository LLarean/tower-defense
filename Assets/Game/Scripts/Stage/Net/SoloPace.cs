using Game.Core.Battles;
using Game.Stage.Orders;

namespace Game.Stage.Net
{
    public sealed class SoloPace : IPace
    {
        private readonly Timeline _timeline;
        private readonly Clock _clock = new Clock();

        public SoloPace(Timeline timeline)
        {
            _timeline = timeline;
            Post = new LocalPost(timeline);
        }

        public IPost Post { get; }

        public bool Desynced => false;

        public void Advance(float seconds) => _clock.Run(seconds, _timeline);

        public void Dispose()
        {
        }
    }
}

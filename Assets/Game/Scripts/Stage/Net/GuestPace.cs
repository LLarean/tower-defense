using Game.Core.Battles;
using Game.Core.Lockstep;
using Game.Stage.Orders;
using Unity.Netcode;

namespace Game.Stage.Net
{
    public sealed class GuestPace : IPace, IPost
    {
        private const int Lag = 6;

        private readonly Guest _guest;
        private readonly NgoLink _link;
        private readonly Clock _clock = new Clock();

        public GuestPace(Timeline timeline, NetworkManager manager)
        {
            _link = new NgoLink(manager);
            _guest = new Guest(timeline, _link);
            _link.Listen(_guest.Receive);
        }

        public IPost Post => this;

        public bool Desynced => _guest.Desynced;

        public void Send(Order order) => _guest.Send(order);

        public void Advance(float seconds)
        {
            var timeline = _guest.Timeline;
            _clock.Run(seconds, timeline);
            while (timeline.Horizon - timeline.Tick > Lag && timeline.TryStep())
            {
            }
            _guest.Pump();
        }

        public void Dispose() => _link.Dispose();
    }
}

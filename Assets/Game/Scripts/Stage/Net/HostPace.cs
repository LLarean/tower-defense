using Game.Core.Battles;
using Game.Core.Lockstep;
using Game.Stage.Orders;
using Unity.Netcode;

namespace Game.Stage.Net
{
    public sealed class HostPace : IPace, IPost
    {
        private const int Delay = 4;

        private readonly Host _host;
        private readonly NgoLink _link;
        private readonly Clock _clock = new Clock();

        public HostPace(Timeline timeline, NetworkManager manager)
        {
            _link = new NgoLink(manager);
            _host = new Host(timeline, _link, Delay);
            _link.Listen(_host.Receive);
        }

        public IPost Post => this;

        public bool Desynced => _host.Desynced;

        public void Send(Order order) => _host.Accept(order);

        public void Advance(float seconds)
        {
            _clock.Run(seconds, _host.Timeline);
            _host.Pump();
        }

        public void Dispose() => _link.Dispose();
    }
}

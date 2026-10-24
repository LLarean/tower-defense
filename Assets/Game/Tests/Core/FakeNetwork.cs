using System.Collections.Generic;
using Game.Core.Lockstep;

namespace Game.Core.Tests
{
    public sealed class FakeNetwork
    {
        private const int Latency = 3;

        private readonly List<(int Due, int Peer, byte[] Data)> _flight = new List<(int, int, byte[])>();
        private Host _host;
        private Guest[] _guests = new Guest[0];
        private int _frame;

        public FakeNetwork(int peers)
        {
            Downlink = new Down(this, peers);
        }

        public IDownlink Downlink { get; }

        public IUplink Uplink() => new Up(this);

        public void Connect(Host host, Guest[] guests)
        {
            _host = host;
            _guests = guests;
        }

        public void Deliver(int frame)
        {
            _frame = frame;
            for (var index = 0; index < _flight.Count; index++)
            {
                var (due, peer, data) = _flight[index];
                if (due > frame)
                {
                    continue;
                }
                _flight.RemoveAt(index--);
                if (peer == 0)
                {
                    _host.Receive(data);
                }
                else
                {
                    _guests[peer - 1].Receive(data);
                }
            }
        }

        private void Post(int peer, byte[] data) => _flight.Add((_frame + Latency, peer, data));

        private sealed class Down : IDownlink
        {
            private readonly FakeNetwork _network;
            private readonly int _peers;

            public Down(FakeNetwork network, int peers)
            {
                _network = network;
                _peers = peers;
            }

            public void Broadcast(byte[] data)
            {
                for (var peer = 1; peer < _peers; peer++)
                {
                    _network.Post(peer, data);
                }
            }
        }

        private sealed class Up : IUplink
        {
            private readonly FakeNetwork _network;

            public Up(FakeNetwork network)
            {
                _network = network;
            }

            public void Send(byte[] data) => _network.Post(0, data);
        }
    }
}

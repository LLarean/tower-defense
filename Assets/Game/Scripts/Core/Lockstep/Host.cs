using System.Collections.Generic;
using Game.Core.Battles;

namespace Game.Core.Lockstep
{
    public sealed class Host
    {
        public const int Interval = 30;

        private readonly IDownlink _downlink;
        private readonly int _delay;
        private readonly List<(int Tick, Order Order)> _outgoing = new List<(int, Order)>();
        private readonly Dictionary<int, long> _sums = new Dictionary<int, long>();

        public Host(Timeline timeline, IDownlink downlink, int delay)
        {
            Timeline = timeline;
            _downlink = downlink;
            _delay = delay;
        }

        public Timeline Timeline { get; }

        public bool Desynced { get; private set; }

        public void Accept(Order order)
        {
            var tick = Timeline.Tick + _delay;
            Timeline.Schedule(tick, order);
            _outgoing.Add((tick, order));
        }

        public void Receive(byte[] data)
        {
            var packet = Packet.Decode(data);
            if (packet.Kind == PacketKind.Command)
            {
                Accept(packet.Orders[0].Order);
            }
            else if (packet.Kind == PacketKind.Sum && _sums.TryGetValue(packet.Tick, out var sum) && sum != packet.Checksum && !Desynced)
            {
                Desynced = true;
                _downlink.Broadcast(Packet.Desync(packet.Tick).Encode());
            }
        }

        public void Pump()
        {
            var tick = Timeline.Tick;
            if (tick % Interval == 0 && !_sums.ContainsKey(tick))
            {
                _sums.Add(tick, Timeline.Checksum());
            }
            _downlink.Broadcast(Packet.Turn(tick + _delay, _outgoing.ToArray()).Encode());
            _outgoing.Clear();
        }
    }
}

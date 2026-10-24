using Game.Core.Battles;

namespace Game.Core.Lockstep
{
    public sealed class Guest
    {
        private readonly IUplink _uplink;
        private int _reported = -1;

        public Guest(Timeline timeline, IUplink uplink)
        {
            Timeline = timeline;
            _uplink = uplink;
            Timeline.Confirm(0);
        }

        public Timeline Timeline { get; }

        public bool Desynced { get; private set; }

        public void Send(Order order) => _uplink.Send(Packet.Command(order).Encode());

        public void Receive(byte[] data)
        {
            var packet = Packet.Decode(data);
            if (packet.Kind == PacketKind.Turn)
            {
                foreach (var (tick, order) in packet.Orders)
                {
                    Timeline.Schedule(tick, order);
                }
                Timeline.Confirm(packet.Tick);
            }
            else if (packet.Kind == PacketKind.Desync)
            {
                Desynced = true;
            }
        }

        public void Pump()
        {
            var tick = Timeline.Tick;
            if (tick % Host.Interval == 0 && tick != _reported)
            {
                _reported = tick;
                _uplink.Send(Packet.Sum(tick, Timeline.Checksum()).Encode());
            }
        }
    }
}

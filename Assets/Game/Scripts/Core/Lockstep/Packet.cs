using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Game.Core.Battles;

namespace Game.Core.Lockstep
{
    public sealed class Packet
    {
        private static readonly IReadOnlyList<(int Tick, Order Order)> None = new (int, Order)[0];

        private Packet(PacketKind kind, int tick, long checksum, IReadOnlyList<(int Tick, Order Order)> orders)
        {
            Kind = kind;
            Tick = tick;
            Checksum = checksum;
            Orders = orders;
        }

        public PacketKind Kind { get; }

        public int Tick { get; }

        public long Checksum { get; }

        public IReadOnlyList<(int Tick, Order Order)> Orders { get; }

        public static Packet Turn(int horizon, IReadOnlyList<(int Tick, Order Order)> orders) =>
            new Packet(PacketKind.Turn, horizon, 0L, orders);

        public static Packet Command(Order order) =>
            new Packet(PacketKind.Command, 0, 0L, new[] { (0, order) });

        public static Packet Sum(int tick, long checksum) => new Packet(PacketKind.Sum, tick, checksum, None);

        public static Packet Desync(int tick) => new Packet(PacketKind.Desync, tick, 0L, None);

        public static Packet Decode(byte[] data)
        {
            using var reader = new BinaryReader(new MemoryStream(data));
            var kind = (PacketKind)reader.ReadByte();
            var tick = reader.ReadInt32();
            var checksum = reader.ReadInt64();
            var orders = new (int Tick, Order Order)[reader.ReadInt32()];
            for (var index = 0; index < orders.Length; index++)
            {
                var when = reader.ReadInt32();
                var verb = (Verb)reader.ReadByte();
                var player = reader.ReadInt32();
                var subject = reader.ReadInt32();
                var point = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                orders[index] = (when, new Order(verb, player, subject, point));
            }
            return new Packet(kind, tick, checksum, orders);
        }

        public byte[] Encode()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            writer.Write((byte)Kind);
            writer.Write(Tick);
            writer.Write(Checksum);
            writer.Write(Orders.Count);
            foreach (var (when, order) in Orders)
            {
                writer.Write(when);
                writer.Write((byte)order.Verb);
                writer.Write(order.Player);
                writer.Write(order.Subject);
                writer.Write(order.Point.X);
                writer.Write(order.Point.Y);
                writer.Write(order.Point.Z);
            }
            writer.Flush();
            return stream.ToArray();
        }
    }
}

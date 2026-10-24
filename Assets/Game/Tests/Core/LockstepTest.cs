using System.Collections.Generic;
using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Battles;
using Game.Core.Economy;
using Game.Core.Elements;
using Game.Core.Lockstep;
using Game.Core.Monsters;
using Game.Core.Paths;
using Game.Core.Reactions;
using Game.Core.Towers;
using Game.Core.Waves;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class LockstepTest
    {
        private static readonly Breed Grunt = new Breed("Grunt", 80f, 0f, 4f, 5, 1);
        private static readonly TowerSpec Fire = new TowerSpec("Fire", 30f, new[] { Element.Fire }, new Grade(10f, 3f, 3f, 9f, 0.5f, 50), new Grade(20f, 5f, 3f, 9f, 0.5f, 60));
        private static readonly TowerSpec Water = new TowerSpec("Water", 30f, new[] { Element.Water }, new Grade(5f, 1f, 3f, 9f, 0.4f, 40));

        [Test]
        public void EncodesAndDecodesTurn()
        {
            var turn = Packet.Turn(42, new List<(int, Order)> { (40, Order.Build(1, 0, new Vector3(1f, 2f, 3f))) });
            var decoded = Packet.Decode(turn.Encode());
            Assert.That(decoded.Kind, Is.EqualTo(PacketKind.Turn));
            Assert.That(decoded.Tick, Is.EqualTo(42));
            Assert.That(decoded.Orders[0].Item2.Point, Is.EqualTo(new Vector3(1f, 2f, 3f)));
        }

        [Test]
        public void KeepsAllPeersInSyncDespiteLatency()
        {
            var network = new FakeNetwork(3);
            var host = new Host(Timeline(), network.Downlink, 4);
            var guests = new[] { new Guest(Timeline(), network.Uplink()), new Guest(Timeline(), network.Uplink()) };
            network.Connect(host, guests);
            host.Accept(Order.Summon(0));
            for (var frame = 0; frame < 600; frame++)
            {
                if (frame == 30)
                {
                    host.Accept(Order.Build(0, 0, new Vector3(9f, 0f, 3f)));
                    guests[0].Send(Order.Build(1, 1, new Vector3(15f, 0f, 3f)));
                }
                if (frame == 200)
                {
                    guests[1].Send(Order.Build(2, 0, new Vector3(21f, 0f, 3f)));
                    guests[0].Send(Order.Upgrade(1, 1));
                }
                host.Timeline.TryStep();
                host.Pump();
                network.Deliver(frame);
                foreach (var guest in guests)
                {
                    guest.Timeline.TryStep();
                    guest.Pump();
                }
            }
            while (guests[0].Timeline.TryStep() | guests[1].Timeline.TryStep())
            {
            }
            Assert.That(host.Timeline.Battle.Garrison.Towers.Count, Is.EqualTo(3));
            Assert.That(guests[0].Timeline.Tick, Is.EqualTo(guests[1].Timeline.Tick));
            Assert.That(guests[0].Timeline.Checksum(), Is.EqualTo(guests[1].Timeline.Checksum()));
            Assert.That(host.Desynced, Is.False);
        }

        [Test]
        public void StallsGuestUntilTurnArrives()
        {
            var network = new FakeNetwork(2);
            var host = new Host(Timeline(), network.Downlink, 4);
            var guest = new Guest(Timeline(), network.Uplink());
            network.Connect(host, new[] { guest });
            for (var tick = 0; tick < 10; tick++)
            {
                guest.Timeline.TryStep();
            }
            Assert.That(guest.Timeline.Tick, Is.EqualTo(0));
        }

        [Test]
        public void ReportsDesyncWhenGuestDiverges()
        {
            var network = new FakeNetwork(2);
            var host = new Host(Timeline(), network.Downlink, 2);
            var guest = new Guest(Timeline(), network.Uplink());
            network.Connect(host, new[] { guest });
            host.Accept(Order.Summon(0));
            guest.Timeline.Schedule(5, Order.Build(1, 0, new Vector3(9f, 0f, 3f)));
            for (var frame = 0; frame < 200; frame++)
            {
                host.Timeline.TryStep();
                host.Pump();
                network.Deliver(frame);
                guest.Timeline.TryStep();
                guest.Pump();
                network.Deliver(frame);
            }
            Assert.That(host.Desynced, Is.True);
        }

        private static Timeline Timeline()
        {
            var treasury = new Treasury(3, 300);
            var lives = new Lives(20);
            var chemistry = new Chemistry().With(Element.Fire, Element.Water, new ThermalShock(2f));
            var arena = new Arena(chemistry, new Horde(4f), new Ledger(treasury, lives));
            var route = new Route(Vector3.Zero, new Vector3(30f, 0f, 0f), new Vector3(30f, 0f, 30f));
            var invasion = new Invasion(new[] { route }, 2f, new Wave(new Squad(Grunt, 12, 0.4f, 0f, 0)));
            var buildable = new bool[20, 3];
            for (var x = 0; x < 20; x++)
            {
                buildable[x, 1] = true;
            }
            var garrison = new Garrison(new Plots(Vector3.Zero, 2f, buildable), treasury);
            return new Timeline(new Battle(arena, invasion, garrison, lives), new[] { Fire, Water }, treasury);
        }
    }
}

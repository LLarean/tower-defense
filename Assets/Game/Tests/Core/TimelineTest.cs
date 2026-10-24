using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Battles;
using Game.Core.Economy;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Paths;
using Game.Core.Reactions;
using Game.Core.Towers;
using Game.Core.Waves;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class TimelineTest
    {
        private static readonly Breed Grunt = new Breed("Grunt", 60f, 0f, 4f, 5, 1);
        private static readonly TowerSpec Fire = new TowerSpec(
            "Fire",
            30f,
            new[] { Element.Fire },
            new Grade(10f, 3f, 3f, 8f, 0.5f, 50),
            new Grade(20f, 5f, 3f, 9f, 0.5f, 60));

        [Test]
        public void AppliesOrderOnItsScheduledTick()
        {
            var timeline = Timeline();
            timeline.Schedule(3, Order.Build(0, 0, new Vector3(9f, 0f, 3f)));
            Run(timeline, 3);
            Assert.That(timeline.Battle.Garrison.Towers, Is.Empty);
            Run(timeline, 1);
            Assert.That(timeline.Battle.Garrison.Towers.Count, Is.EqualTo(1));
        }

        [Test]
        public void WaitsForConfirmationBeforeAdvancing()
        {
            var timeline = Timeline();
            timeline.Confirm(2);
            Assert.That(Run(timeline, 5), Is.EqualTo(2));
        }

        [Test]
        public void FindsTowerByIdForUpgrade()
        {
            var timeline = Timeline();
            timeline.Schedule(0, Order.Build(0, 0, new Vector3(9f, 0f, 3f)));
            timeline.Schedule(1, Order.Upgrade(0, 0));
            Run(timeline, 2);
            Assert.That(timeline.Battle.Garrison.Towers[0].Level, Is.EqualTo(1));
        }

        [Test]
        public void GivesSameChecksumForSameOrders()
        {
            var first = Timeline();
            var second = Timeline();
            foreach (var timeline in new[] { first, second })
            {
                timeline.Schedule(0, Order.Build(0, 0, new Vector3(9f, 0f, 3f)));
                timeline.Schedule(1, Order.Summon(0));
                Run(timeline, 300);
            }
            Assert.That(first.Checksum(), Is.EqualTo(second.Checksum()));
        }

        [Test]
        public void GivesDifferentChecksumForDifferentOrders()
        {
            var first = Timeline();
            var second = Timeline();
            first.Schedule(0, Order.Summon(0));
            second.Schedule(0, Order.Build(0, 0, new Vector3(9f, 0f, 3f)));
            second.Schedule(0, Order.Summon(0));
            Run(first, 200);
            Run(second, 200);
            Assert.That(first.Checksum(), Is.Not.EqualTo(second.Checksum()));
        }

        private static Timeline Timeline()
        {
            var treasury = new Treasury(2, 200);
            var lives = new Lives(10);
            var arena = new Arena(new Chemistry(), new Horde(4f), new Ledger(treasury, lives));
            var route = new Route(Vector3.Zero, new Vector3(60f, 0f, 0f));
            var invasion = new Invasion(new[] { route }, 2f, new Wave(new Squad(Grunt, 5, 0.5f, 0f, 0)));
            var buildable = new bool[30, 3];
            for (var x = 0; x < 30; x++)
            {
                buildable[x, 1] = true;
            }
            var garrison = new Garrison(new Plots(Vector3.Zero, 2f, buildable), treasury);
            return new Timeline(new Battle(arena, invasion, garrison, lives), new[] { Fire }, treasury);
        }

        private static int Run(Timeline timeline, int ticks)
        {
            var done = 0;
            for (var tick = 0; tick < ticks && timeline.TryStep(); tick++)
            {
                done++;
            }
            return done;
        }
    }
}

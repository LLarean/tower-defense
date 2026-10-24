using System.Numerics;
using Game.Core.Monsters;
using Game.Core.Paths;
using Game.Core.Waves;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class InvasionTest
    {
        private static readonly Breed Grunt = new Breed("Grunt", 50f, 0f, 1f, 5, 1);
        private static readonly Route Road = new Route(Vector3.Zero, new Vector3(100f, 0f, 0f));
        private static readonly Route Lane = new Route(new Vector3(0f, 0f, 10f), new Vector3(100f, 0f, 10f));

        [Test]
        public void ReleasesSquadMembersOneByOne()
        {
            var field = new Field();
            var invasion = new Invasion(new[] { Road }, 0f, new Wave(new Squad(Grunt, 3, 1f, 0f, 0)));
            Run(invasion, field, 1.5f);
            Assert.That(field.Arena.Horde.Monsters.Count, Is.EqualTo(2));
        }

        [Test]
        public void HoldsSquadUntilItsDelayPasses()
        {
            var field = new Field();
            var invasion = new Invasion(new[] { Road }, 0f, new Wave(new Squad(Grunt, 1, 1f, 2f, 0)));
            Run(invasion, field, 1f);
            Assert.That(field.Arena.Horde.Monsters, Is.Empty);
        }

        [Test]
        public void SendsSquadAlongItsRoute()
        {
            var field = new Field();
            var invasion = new Invasion(new[] { Road, Lane }, 0f, new Wave(new Squad(Grunt, 1, 1f, 0f, 1)));
            Run(invasion, field, 0.1f);
            Assert.That(field.Arena.Horde.Monsters[0].Position.Z, Is.EqualTo(10f));
        }

        [Test]
        public void RestsBetweenWaves()
        {
            var field = new Field();
            var invasion = new Invasion(new[] { Road }, 5f, new Wave(new Squad(Grunt, 1, 1f, 0f, 0)), new Wave(new Squad(Grunt, 1, 1f, 0f, 0)));
            Run(invasion, field, 3f);
            Assert.That(invasion.Wave, Is.EqualTo(1));
            Run(invasion, field, 4f);
            Assert.That(invasion.Wave, Is.EqualTo(2));
        }

        [Test]
        public void SummonsNextWaveEarly()
        {
            var field = new Field();
            var invasion = new Invasion(new[] { Road }, 30f, new Wave(new Squad(Grunt, 1, 1f, 0f, 0)), new Wave(new Squad(Grunt, 1, 1f, 0f, 0)));
            Run(invasion, field, 1f);
            invasion.Summon();
            Run(invasion, field, 0.1f);
            Assert.That(field.Arena.Horde.Monsters.Count, Is.EqualTo(2));
        }

        [Test]
        public void EndsWhenAllWavesAreReleased()
        {
            var field = new Field();
            var invasion = new Invasion(new[] { Road }, 0f, new Wave(new Squad(Grunt, 2, 1f, 0f, 0)));
            Run(invasion, field, 3f);
            Assert.That(invasion.Exhausted, Is.True);
        }

        private static void Run(Invasion invasion, Field field, float seconds)
        {
            const float step = 0.1f;
            for (var time = 0f; time < seconds - 0.001f; time += step)
            {
                invasion.Tick(step, field.Arena.Horde);
            }
        }
    }
}

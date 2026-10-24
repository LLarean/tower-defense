using System.Collections.Generic;
using System.Numerics;
using Game.Core.Elements;
using Game.Core.Monsters;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class HordeTest
    {
        [Test]
        public void ReportsAndRemovesDeadMonsters()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Wound(1000f, Element.Fire, field.Arena);
            field.Tick(0.1f);
            Assert.That(field.Feedback.Dead, Is.EquivalentTo(new[] { monster }));
            Assert.That(field.Arena.Horde.Monsters, Is.Empty);
        }

        [Test]
        public void ReportsMonstersThatReachTheEnd()
        {
            var field = new Field();
            var monster = field.Spawn(99.5f);
            field.Tick(1f);
            Assert.That(field.Feedback.Gone, Is.EquivalentTo(new[] { monster }));
        }

        [Test]
        public void FindsOnlyMonstersWithinRadius()
        {
            var field = new Field();
            var near = field.Spawn(10f);
            field.Spawn(20f);
            var found = new List<Monster>();
            field.Arena.Horde.Near(new Vector3(11f, 0f, 0f), 2f, found);
            Assert.That(found, Is.EquivalentTo(new[] { near }));
        }
    }
}

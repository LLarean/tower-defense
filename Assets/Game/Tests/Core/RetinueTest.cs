using System.Numerics;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Paths;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class RetinueTest
    {
        private static readonly Breed Minion = new Breed("Minion", 10f, 0f, 1f, 1, 1);

        [Test]
        public void CallsFollowersWhenHealthDropsBelowThreshold()
        {
            var field = new Field();
            var boss = Boss(field, 3, 1);
            boss.Wound(60f, Element.Fire, field.Arena);
            field.Tick(0.01f);
            Assert.That(field.Arena.Horde.Monsters.Count, Is.EqualTo(4));
        }

        [Test]
        public void StaysAloneAboveThreshold()
        {
            var field = new Field();
            var boss = Boss(field, 3, 1);
            boss.Wound(40f, Element.Fire, field.Arena);
            field.Tick(0.01f);
            Assert.That(field.Arena.Horde.Monsters.Count, Is.EqualTo(1));
        }

        [Test]
        public void AnswersEveryCrossedThresholdOnce()
        {
            var field = new Field();
            var boss = Boss(field, 2, 3);
            boss.Wound(80f, Element.Fire, field.Arena);
            boss.Wound(1f, Element.Fire, field.Arena);
            field.Tick(0.01f);
            Assert.That(field.Arena.Horde.Monsters.Count, Is.EqualTo(7));
        }

        [Test]
        public void PlacesFollowersBehindTheLeader()
        {
            var field = new Field();
            var boss = Boss(field, 1, 1);
            boss.Push(-50f);
            boss.Wound(60f, Element.Fire, field.Arena);
            field.Tick(0.01f);
            Assert.That(field.Arena.Horde.Monsters[1].Distance, Is.EqualTo(48f).Within(0.1f));
        }

        [Test]
        public void CallsNobodyAfterDeath()
        {
            var field = new Field();
            var boss = Boss(field, 3, 1);
            boss.Wound(500f, Element.Fire, field.Arena);
            field.Tick(0.01f);
            Assert.That(field.Arena.Horde.Monsters.Count, Is.EqualTo(0));
        }

        private static Monster Boss(Field field, int count, int calls)
        {
            var breed = new Breed("Boss", 100f, 0f, 1f, 50, 10).Leading(new Retinue(Minion, count, calls, 2f));
            var boss = new Monster(breed, new Route(new Vector3(0, 0, 0), new Vector3(100, 0, 0)));
            field.Arena.Horde.Add(boss);
            return boss;
        }
    }
}

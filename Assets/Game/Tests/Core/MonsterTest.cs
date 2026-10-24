using Game.Core.Elements;
using Game.Core.Monsters;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class MonsterTest
    {
        [Test]
        public void WalksAlongRouteAtBreedSpeed()
        {
            var field = new Field();
            var monster = field.Spawn();
            field.Tick(1.5f);
            Assert.That(monster.Distance, Is.EqualTo(1.5f).Within(0.001f));
        }

        [Test]
        public void BurnsOverTimeAfterFireHit()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Fire, 0f, 4f, 3f), field.Arena);
            field.Tick(1f);
            Assert.That(monster.Vitality, Is.EqualTo(0.96f).Within(0.001f));
        }

        [Test]
        public void IgnoresElementItResists()
        {
            var field = new Field();
            var monster = field.Spawn(immunities: Element.Fire);
            monster.Take(new Hit(Element.Fire, 50f, 4f, 3f), field.Arena);
            Assert.That(monster.Vitality, Is.EqualTo(1f));
            Assert.That(monster.Has(Condition.Burning), Is.False);
        }

        [Test]
        public void SlowsDownWhenChilled()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Ice, 0f, 0.5f, 3f), field.Arena);
            field.Tick(1f);
            Assert.That(monster.Distance, Is.EqualTo(0.5f).Within(0.001f));
        }

        [Test]
        public void StandsStillWhileStunnedByCharge()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Lightning, 0f, 1f, 3f), field.Arena);
            field.Tick(0.5f);
            Assert.That(monster.Distance, Is.EqualTo(0f));
        }

        [Test]
        public void TakesExtraDamageWhenSundered()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Earth, 0f, 0.5f, 3f), field.Arena);
            monster.Wound(10f, Element.Fire, field.Arena);
            Assert.That(monster.Vitality, Is.EqualTo(0.85f).Within(0.001f));
        }

        [Test]
        public void ArmorAbsorbsPartOfDamage()
        {
            var field = new Field();
            var monster = field.Spawn(armor: 0.5f);
            monster.Wound(20f, Element.Fire, field.Arena);
            Assert.That(monster.Vitality, Is.EqualTo(0.9f).Within(0.001f));
        }

        [Test]
        public void CorrosionStripsArmor()
        {
            var field = new Field();
            var monster = field.Spawn(armor: 0.5f);
            monster.Inflict(Condition.Corroded, 0.5f, 3f);
            monster.Wound(20f, Element.Fire, field.Arena);
            Assert.That(monster.Vitality, Is.EqualTo(0.8f).Within(0.001f));
        }

        [Test]
        public void LosesConditionWhenItExpires()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Water, 0f, 1f, 2f), field.Arena);
            field.Tick(2.5f);
            Assert.That(monster.Has(Condition.Wet), Is.False);
        }

        [Test]
        public void StacksPoisonUpToFiveDoses()
        {
            var field = new Field();
            var monster = field.Spawn();
            for (var dose = 0; dose < 8; dose++)
            {
                monster.Take(new Hit(Element.Poison, 0f, 1f, 5f), field.Arena);
            }
            field.Tick(1f);
            Assert.That(monster.Vitality, Is.EqualTo(0.95f).Within(0.001f));
        }
    }
}

using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Core.Zones;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class ReactionTest
    {
        [Test]
        public void FreezesWetMonsterHitByIce()
        {
            var field = new Field(new Chemistry().With(Element.Water, Element.Ice, new Freeze(2f)));
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Water, 0f, 1f, 3f), field.Arena);
            monster.Take(new Hit(Element.Ice, 0f, 0.5f, 3f), field.Arena);
            field.Tick(1f);
            Assert.That(monster.Distance, Is.EqualTo(0f));
            Assert.That(monster.Has(Condition.Wet), Is.False);
            Assert.That(monster.Has(Condition.Chilled), Is.False);
        }

        [Test]
        public void ShattersFrozenMonsterAndHurtsNeighbors()
        {
            var field = new Field(new Chemistry().With(Condition.Frozen, Element.Earth, new Shatter(3f, 4f, 0.5f)));
            var frozen = field.Spawn(10f);
            var neighbor = field.Spawn(11f);
            frozen.Inflict(Condition.Frozen, 1f, 2f);
            frozen.Take(new Hit(Element.Earth, 10f, 0f, 1f), field.Arena);
            Assert.That(frozen.Vitality, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(neighbor.Vitality, Is.EqualTo(0.8f).Within(0.001f));
        }

        [Test]
        public void ElectrocutesWetMonstersAroundTarget()
        {
            var field = new Field(new Chemistry().With(Element.Water, Element.Lightning, new Electrocute(5f, 1f)));
            var target = field.Spawn(10f);
            var wet = field.Spawn(13f);
            var dry = field.Spawn(12f);
            var far = field.Spawn(30f);
            foreach (var monster in new[] { target, wet, far })
            {
                monster.Take(new Hit(Element.Water, 0f, 1f, 3f), field.Arena);
            }
            target.Take(new Hit(Element.Lightning, 10f, 0f, 1f), field.Arena);
            Assert.That(wet.Vitality, Is.EqualTo(0.9f).Within(0.001f));
            Assert.That(dry.Vitality, Is.EqualTo(1f));
            Assert.That(far.Vitality, Is.EqualTo(1f));
            Assert.That(field.Feedback.Chains, Is.EqualTo(1));
        }

        [Test]
        public void ElectrocutesChargedMonsterHitByWater()
        {
            var field = new Field(new Chemistry().With(Element.Water, Element.Lightning, new Electrocute(5f, 1f)));
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Lightning, 0f, 0f, 3f), field.Arena);
            monster.Take(new Hit(Element.Water, 10f, 1f, 3f), field.Arena);
            Assert.That(field.Feedback.Reactions, Is.EquivalentTo(new[] { ReactionKind.Electrocute }));
        }

        [Test]
        public void ConsumesBothElementsInThermalShock()
        {
            var field = new Field(new Chemistry().With(Element.Fire, Element.Ice, new ThermalShock(3f)));
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Fire, 0f, 1f, 3f), field.Arena);
            monster.Take(new Hit(Element.Ice, 10f, 0.5f, 3f), field.Arena);
            Assert.That(monster.Vitality, Is.EqualTo(0.6f).Within(0.001f));
            Assert.That(monster.Has(Condition.Burning), Is.False);
            Assert.That(monster.Has(Condition.Chilled), Is.False);
        }

        [Test]
        public void LeavesSteamThatScaldsMonstersPassingThrough()
        {
            var steam = new Pool(ReactionKind.Steam, 3f, 2f, new Scald(Element.Fire, 10f));
            var field = new Field(new Chemistry().With(Element.Fire, Element.Water, steam));
            var target = field.Spawn(10f);
            var passer = field.Spawn(11f);
            target.Take(new Hit(Element.Water, 0f, 1f, 3f), field.Arena);
            target.Take(new Hit(Element.Fire, 0f, 1f, 3f), field.Arena);
            field.Tick(1f);
            Assert.That(passer.Vitality, Is.EqualTo(0.9f).Within(0.001f));
        }

        [Test]
        public void IgnitesNeighborsInGasExplosion()
        {
            var field = new Field(new Chemistry().With(Element.Fire, Element.Poison, new Ignition(ReactionKind.GasExplosion, 3f, 1f, 3f)));
            var target = field.Spawn(10f);
            var neighbor = field.Spawn(12f);
            target.Take(new Hit(Element.Poison, 0f, 1f, 3f), field.Arena);
            target.Take(new Hit(Element.Fire, 10f, 2f, 3f), field.Arena);
            Assert.That(neighbor.Has(Condition.Burning), Is.True);
        }

        [Test]
        public void PushesMonstersBackInOverload()
        {
            var field = new Field(new Chemistry().With(Element.Fire, Element.Lightning, new Overload(3f, 0f, 4f)));
            var target = field.Spawn(10f);
            target.Take(new Hit(Element.Fire, 0f, 1f, 3f), field.Arena);
            target.Take(new Hit(Element.Lightning, 0f, 0f, 3f), field.Arena);
            Assert.That(target.Distance, Is.EqualTo(6f).Within(0.001f));
        }
    }
}

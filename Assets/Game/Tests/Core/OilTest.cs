using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Core.Zones;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class OilTest
    {
        [Test]
        public void SlowsMonsterDownWhenOiled()
        {
            var field = new Field();
            var monster = field.Spawn();
            monster.Take(new Hit(Element.Oil, 0f, 0.4f, 3f), field.Arena);
            field.Tick(1f);
            Assert.That(monster.Has(Condition.Oiled), Is.True);
            Assert.That(monster.Distance, Is.EqualTo(0.6f).Within(0.001f));
        }

        [Test]
        public void BlazesOiledCrowdWhenFireArrives()
        {
            var field = new Field(new Chemistry().With(Element.Oil, Element.Fire, new Ignition(ReactionKind.Blaze, 4f, 3f, 5f)));
            var target = field.Spawn(10f);
            var neighbor = field.Spawn(13f);
            target.Take(new Hit(Element.Oil, 0f, 0.3f, 3f), field.Arena);
            target.Take(new Hit(Element.Fire, 10f, 2f, 3f), field.Arena);
            Assert.That(neighbor.Vitality, Is.EqualTo(0.7f).Within(0.001f));
            Assert.That(neighbor.Has(Condition.Burning), Is.True);
            Assert.That(field.Feedback.Reactions, Does.Contain(ReactionKind.Blaze));
        }

        [Test]
        public void SpillsSlickThatOilsPassers()
        {
            var slick = new Pool(ReactionKind.Slick, 3f, 4f, new Affliction(Condition.Oiled, 0.3f));
            var field = new Field(new Chemistry().With(Element.Oil, Element.Water, slick));
            var target = field.Spawn(10f);
            var passer = field.Spawn(11f);
            target.Take(new Hit(Element.Water, 0f, 1f, 3f), field.Arena);
            target.Take(new Hit(Element.Oil, 0f, 0.3f, 3f), field.Arena);
            field.Tick(0.1f);
            Assert.That(passer.Has(Condition.Oiled), Is.True);
        }
    }
}

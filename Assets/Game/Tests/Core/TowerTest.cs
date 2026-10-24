using System.Numerics;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Core.Towers;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class TowerTest
    {
        private static readonly Grade Basic = new Grade(10f, 1f, 3f, 5f, 1f, 50);
        private static readonly Grade Strong = new Grade(25f, 2f, 3f, 6f, 1f, 80);

        [Test]
        public void ShootsMonsterWithinRange()
        {
            var field = new Field();
            var monster = field.Spawn(10f);
            var tower = new Tower(0, Spec(Element.Fire), new Vector3(10f, 0f, 3f), 0);
            var volley = new Volley();
            Battle(field, tower, volley, 2f);
            Assert.That(monster.Vitality, Is.LessThan(1f));
            Assert.That(monster.Has(Condition.Burning), Is.True);
        }

        [Test]
        public void IgnoresMonsterOutOfRange()
        {
            var field = new Field();
            var monster = field.Spawn(10f);
            var tower = new Tower(0, Spec(Element.Fire), new Vector3(40f, 0f, 0f), 0);
            var volley = new Volley();
            Battle(field, tower, volley, 2f);
            Assert.That(monster.Vitality, Is.EqualTo(1f));
        }

        [Test]
        public void AimsAtMonsterClosestToTheEnd()
        {
            var field = new Field();
            var behind = field.Spawn(9f);
            var ahead = field.Spawn(12f);
            var tower = new Tower(0, Spec(Element.Water), new Vector3(10f, 0f, 2f), 0);
            var volley = new Volley();
            Battle(field, tower, volley, 0.5f);
            Assert.That(ahead.Has(Condition.Wet), Is.True);
            Assert.That(behind.Has(Condition.Wet), Is.False);
        }

        [Test]
        public void WaitsForCooldownBetweenShots()
        {
            var field = new Field();
            field.Spawn(10f);
            var tower = new Tower(0, Spec(Element.Fire), new Vector3(10f, 0f, 1f), 0);
            var volley = new Volley();
            tower.Tick(0.1f, field.Arena, volley);
            tower.Tick(0.1f, field.Arena, volley);
            Assert.That(volley.Missiles.Count, Is.EqualTo(1));
        }

        [Test]
        public void TriggersReactionWithTwoElementsAtOnce()
        {
            var field = new Field(new Chemistry().With(Element.Water, Element.Lightning, new Electrocute(3f, 1f)));
            field.Spawn(10f);
            var tower = new Tower(0, Spec(Element.Water, Element.Lightning), new Vector3(10f, 0f, 1f), 0);
            var volley = new Volley();
            Battle(field, tower, volley, 1f);
            Assert.That(field.Feedback.Reactions, Does.Contain(ReactionKind.Electrocute));
        }

        [Test]
        public void RecoversReadinessDuringCooldown()
        {
            var field = new Field();
            field.Spawn(10f);
            var tower = new Tower(0, Spec(Element.Fire), new Vector3(10f, 0f, 1f), 0);
            var volley = new Volley();
            tower.Tick(0.1f, field.Arena, volley);
            Assert.That(tower.Readiness, Is.EqualTo(0f));
            tower.Tick(0.5f, field.Arena, volley);
            Assert.That(tower.Readiness, Is.EqualTo(0.5f).Within(0.001f));
        }

        [Test]
        public void GrowsStrongerAfterUpgrade()
        {
            var tower = new Tower(0, Spec(Element.Fire), Vector3.Zero, 0);
            tower.Upgrade();
            Assert.That(tower.Grade.Damage, Is.EqualTo(25f));
            Assert.That(tower.Upgradable, Is.False);
        }

        [Test]
        public void ReportsCostOfNextGrade()
        {
            var tower = new Tower(0, Spec(Element.Fire), Vector3.Zero, 0);
            Assert.That(tower.UpgradeCost, Is.EqualTo(80));
        }

        [Test]
        public void MissileFizzlesWhenTargetIsAlreadyDead()
        {
            var field = new Field();
            var monster = field.Spawn(10f);
            var other = field.Spawn(10.5f);
            var tower = new Tower(0, Spec(Element.Fire), new Vector3(10f, 0f, 4f), 0);
            var volley = new Volley();
            tower.Tick(0.1f, field.Arena, volley);
            other.Wound(1000f, Element.Earth, field.Arena);
            monster.Wound(1000f, Element.Earth, field.Arena);
            volley.Tick(5f, field.Arena);
            Assert.That(volley.Missiles, Is.Empty);
        }

        private static TowerSpec Spec(params Element[] elements) => new TowerSpec("Test", 20f, elements, Basic, Strong);

        private static void Battle(Field field, Tower tower, Volley volley, float seconds)
        {
            const float step = 0.05f;
            for (var time = 0f; time < seconds; time += step)
            {
                tower.Tick(step, field.Arena, volley);
                volley.Tick(step, field.Arena);
            }
        }
    }
}

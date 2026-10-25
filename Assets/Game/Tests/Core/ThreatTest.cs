using Game.Core.Monsters;
using Game.Core.Waves;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class ThreatTest
    {
        private static readonly Breed Grunt = new Breed("Grunt", 100f, 0.1f, 3f, 10, 1);

        [Test]
        public void KeepsBreedIntactForLonePlayerOnNormal()
        {
            var breed = new Threat(1f, 1f, 0.7f, 1).Toughen(Grunt);
            Assert.That(breed.Health, Is.EqualTo(100f));
            Assert.That(breed.Bounty, Is.EqualTo(10));
        }

        [Test]
        public void ScalesHealthWithDifficulty()
        {
            var breed = new Threat(1.5f, 1f, 0.7f, 1).Toughen(Grunt);
            Assert.That(breed.Health, Is.EqualTo(150f).Within(0.001f));
        }

        [Test]
        public void ToughensMonstersForEveryExtraPlayer()
        {
            var breed = new Threat(1f, 1f, 0.5f, 3).Toughen(Grunt);
            Assert.That(breed.Health, Is.EqualTo(200f).Within(0.001f));
        }

        [Test]
        public void RaisesBountyWithCrowd()
        {
            var breed = new Threat(1f, 1f, 0.5f, 3).Toughen(Grunt);
            Assert.That(breed.Bounty, Is.EqualTo(20));
        }

        [Test]
        public void KeepsArmorAndSpeedOfBreed()
        {
            var breed = new Threat(2f, 2f, 0.5f, 4).Toughen(Grunt);
            Assert.That(breed.Speed, Is.EqualTo(3f));
            Assert.That(breed.Armor, Is.EqualTo(0.1f));
        }
    }
}

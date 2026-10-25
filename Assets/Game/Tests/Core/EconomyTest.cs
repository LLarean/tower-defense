using Game.Core.Arenas;
using Game.Core.Economy;
using Game.Core.Elements;
using Game.Core.Reactions;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class EconomyTest
    {
        [Test]
        public void SpendsGoldOnlyWhenEnough()
        {
            var wallet = new Wallet(100);
            Assert.That(wallet.TrySpend(150), Is.False);
            Assert.That(wallet.TrySpend(60), Is.True);
            Assert.That(wallet.Gold, Is.EqualTo(40));
        }

        [Test]
        public void SharesBountyAmongAllPlayers()
        {
            var treasury = new Treasury(3, 0);
            treasury.Share(10);
            Assert.That(treasury.Wallet(0).Gold, Is.EqualTo(4));
            Assert.That(treasury.Wallet(2).Gold, Is.EqualTo(4));
        }

        [Test]
        public void LosesLivesButNeverBelowZero()
        {
            var lives = new Lives(3);
            lives.Lose(5);
            Assert.That(lives.Left, Is.EqualTo(0));
            Assert.That(lives.Lost, Is.True);
        }

        [Test]
        public void GainsLives()
        {
            var lives = new Lives(3);
            lives.Gain(2);
            Assert.That(lives.Left, Is.EqualTo(5));
        }

        [Test]
        public void PaysBountyForSlainMonster()
        {
            var treasury = new Treasury(2, 0);
            var ledger = new Ledger(treasury, new Lives(10));
            var field = new Field(new Chemistry(), ledger);
            var monster = field.Spawn();
            monster.Wound(1000f, Element.Fire, field.Arena);
            field.Tick(0.1f);
            Assert.That(treasury.Wallet(1).Gold, Is.EqualTo(3));
        }

        [Test]
        public void TakesLifeForEscapedMonster()
        {
            var lives = new Lives(10);
            var field = new Field(new Chemistry(), new Ledger(new Treasury(1, 0), lives));
            field.Spawn(99.9f);
            field.Tick(1f);
            Assert.That(lives.Left, Is.EqualTo(9));
        }

        [Test]
        public void DeliversEventsToEveryListener()
        {
            var first = new FakeFeedback();
            var second = new FakeFeedback();
            IFeedback feedback = new Feedbacks(first, second);
            feedback.Chained(default, default);
            Assert.That(first.Chains + second.Chains, Is.EqualTo(2));
        }
    }
}

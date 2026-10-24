using System.Numerics;
using Game.Core.Economy;
using Game.Core.Elements;
using Game.Core.Towers;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class GarrisonTest
    {
        private static readonly TowerSpec Fire = new TowerSpec(
            "Fire",
            20f,
            new[] { Element.Fire },
            new Grade(10f, 1f, 3f, 5f, 1f, 50),
            new Grade(20f, 2f, 3f, 6f, 1f, 100));

        [Test]
        public void SnapsTowerToCellCenter()
        {
            var garrison = Garrison(200);
            Assert.That(garrison.TryBuild(0, Fire, new Vector3(2.7f, 0f, 1.2f), out var tower), Is.True);
            Assert.That(tower.Position, Is.EqualTo(new Vector3(3f, 0f, 1f)));
        }

        [Test]
        public void ChargesBuilderForTower()
        {
            var treasury = new Treasury(2, 200);
            var garrison = new Garrison(Plots(), treasury);
            garrison.TryBuild(1, Fire, new Vector3(1f, 0f, 1f), out _);
            Assert.That(treasury.Wallet(1).Gold, Is.EqualTo(150));
            Assert.That(treasury.Wallet(0).Gold, Is.EqualTo(200));
        }

        [Test]
        public void RefusesToBuildWithoutGold()
        {
            var garrison = Garrison(30);
            Assert.That(garrison.TryBuild(0, Fire, new Vector3(1f, 0f, 1f), out _), Is.False);
            Assert.That(garrison.Towers, Is.Empty);
        }

        [Test]
        public void RefusesToBuildOnOccupiedCell()
        {
            var garrison = Garrison(500);
            garrison.TryBuild(0, Fire, new Vector3(1f, 0f, 1f), out _);
            Assert.That(garrison.TryBuild(0, Fire, new Vector3(1.4f, 0f, 0.8f), out _), Is.False);
        }

        [Test]
        public void RefusesToBuildOnBlockedCell()
        {
            var garrison = Garrison(500);
            Assert.That(garrison.TryBuild(0, Fire, new Vector3(5f, 0f, 1f), out _), Is.False);
        }

        [Test]
        public void LetsAnyPlayerPayForUpgrade()
        {
            var treasury = new Treasury(2, 200);
            var garrison = new Garrison(Plots(), treasury);
            garrison.TryBuild(0, Fire, new Vector3(1f, 0f, 1f), out var tower);
            Assert.That(garrison.TryUpgrade(1, tower), Is.True);
            Assert.That(treasury.Wallet(1).Gold, Is.EqualTo(100));
            Assert.That(tower.Level, Is.EqualTo(1));
        }

        [Test]
        public void RefundsOwnerAndFreesCellOnSale()
        {
            var treasury = new Treasury(1, 200);
            var garrison = new Garrison(Plots(), treasury);
            garrison.TryBuild(0, Fire, new Vector3(1f, 0f, 1f), out var tower);
            garrison.TryUpgrade(0, tower);
            Assert.That(garrison.TrySell(0, tower), Is.True);
            Assert.That(treasury.Wallet(0).Gold, Is.EqualTo(155));
            Assert.That(garrison.TryBuild(0, Fire, new Vector3(1f, 0f, 1f), out _), Is.True);
        }

        [Test]
        public void ForbidsSellingSomeoneElsesTower()
        {
            var garrison = new Garrison(Plots(), new Treasury(2, 200));
            garrison.TryBuild(0, Fire, new Vector3(1f, 0f, 1f), out var tower);
            Assert.That(garrison.TrySell(1, tower), Is.False);
        }

        private static Garrison Garrison(int gold) => new Garrison(Plots(), new Treasury(1, gold));

        private static Plots Plots()
        {
            var buildable = new bool[4, 2];
            for (var x = 0; x < 2; x++)
            {
                for (var z = 0; z < 2; z++)
                {
                    buildable[x, z] = true;
                }
            }
            return new Plots(Vector3.Zero, 2f, buildable);
        }
    }
}

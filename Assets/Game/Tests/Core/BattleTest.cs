using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Battles;
using Game.Core.Economy;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Paths;
using Game.Core.Reactions;
using Game.Core.Towers;
using Game.Core.Waves;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class BattleTest
    {
        private static readonly Breed Grunt = new Breed("Grunt", 30f, 0f, 4f, 5, 2);
        private static readonly TowerSpec Cannon = new TowerSpec(
            "Cannon",
            50f,
            new[] { Element.Earth },
            new Grade(100f, 0f, 1f, 8f, 0.2f, 50));

        [Test]
        public void WaitsUntilPlayersBegin()
        {
            var battle = Battle(new Lives(10), out var arena);
            Run(battle, 5f);
            Assert.That(arena.Horde.Monsters, Is.Empty);
            Assert.That(battle.Outcome, Is.EqualTo(Outcome.Ongoing));
        }

        [Test]
        public void WinsWhenTowersHoldTheLine()
        {
            var battle = Battle(new Lives(10), out _);
            battle.Garrison.TryBuild(0, Cannon, new Vector3(10f, 0f, 2f), out _);
            battle.Begin();
            Run(battle, 30f);
            Assert.That(battle.Outcome, Is.EqualTo(Outcome.Won));
        }

        [Test]
        public void LosesWhenMonstersBreakThrough()
        {
            var lives = new Lives(3);
            var battle = Battle(lives, out _);
            battle.Begin();
            Run(battle, 30f);
            Assert.That(battle.Outcome, Is.EqualTo(Outcome.Lost));
            Assert.That(lives.Left, Is.EqualTo(0));
        }

        private static Battle Battle(Lives lives, out Arena arena)
        {
            var treasury = new Treasury(1, 500);
            arena = new Arena(new Chemistry(), new Horde(4f), new Ledger(treasury, lives));
            var route = new Route(Vector3.Zero, new Vector3(40f, 0f, 0f));
            var invasion = new Invasion(new[] { route }, 1f, new Wave(new Squad(Grunt, 3, 0.5f, 0f, 0)));
            var buildable = new bool[20, 2];
            buildable[5, 1] = true;
            var garrison = new Garrison(new Plots(new Vector3(0f, 0f, 0f), 2f, buildable), treasury);
            return new Battle(arena, invasion, garrison, lives);
        }

        private static void Run(Battle battle, float seconds)
        {
            const float step = 0.05f;
            for (var time = 0f; time < seconds; time += step)
            {
                battle.Tick(step);
            }
        }
    }
}

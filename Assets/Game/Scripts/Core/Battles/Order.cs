using System.Collections.Generic;
using System.Numerics;
using Game.Core.Towers;

namespace Game.Core.Battles
{
    public readonly struct Order
    {
        public Order(Verb verb, int player, int subject, Vector3 point)
        {
            Verb = verb;
            Player = player;
            Subject = subject;
            Point = point;
        }

        public Verb Verb { get; }

        public int Player { get; }

        public int Subject { get; }

        public Vector3 Point { get; }

        public static Order Build(int player, int spec, Vector3 point) => new Order(Verb.Build, player, spec, point);

        public static Order Upgrade(int player, int tower) => new Order(Verb.Upgrade, player, tower, Vector3.Zero);

        public static Order Sell(int player, int tower) => new Order(Verb.Sell, player, tower, Vector3.Zero);

        public static Order Summon(int player) => new Order(Verb.Summon, player, 0, Vector3.Zero);

        public void Apply(Battle battle, IReadOnlyList<TowerSpec> arsenal)
        {
            var garrison = battle.Garrison;
            switch (Verb)
            {
                case Verb.Build when Subject >= 0 && Subject < arsenal.Count:
                    garrison.TryBuild(Player, arsenal[Subject], Point, out _);
                    break;
                case Verb.Upgrade when garrison.TryFind(Subject, out var tower):
                    garrison.TryUpgrade(Player, tower);
                    break;
                case Verb.Sell when garrison.TryFind(Subject, out var tower):
                    garrison.TrySell(Player, tower);
                    break;
                case Verb.Summon:
                    battle.Summon();
                    break;
            }
        }
    }
}

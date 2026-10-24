using System;
using System.Collections.Generic;
using Game.Core.Economy;
using Game.Core.Towers;

namespace Game.Core.Battles
{
    public sealed class Timeline
    {
        public const float Step = 1f / 30f;

        private const long Prime = 1099511628211L;
        private const float Precision = 1000f;

        private readonly IReadOnlyList<TowerSpec> _arsenal;
        private readonly Treasury _treasury;
        private readonly Dictionary<int, List<Order>> _orders = new Dictionary<int, List<Order>>();
        private int _horizon = int.MaxValue;

        public Timeline(Battle battle, IReadOnlyList<TowerSpec> arsenal, Treasury treasury)
        {
            Battle = battle;
            _arsenal = arsenal;
            _treasury = treasury;
        }

        public Battle Battle { get; }

        public int Tick { get; private set; }

        public int Horizon => _horizon;

        public void Schedule(int tick, Order order)
        {
            if (!_orders.TryGetValue(tick, out var orders))
            {
                orders = new List<Order>();
                _orders.Add(tick, orders);
            }
            orders.Add(order);
        }

        public void Confirm(int tick) => _horizon = tick;

        public bool TryStep()
        {
            if (Tick >= _horizon)
            {
                return false;
            }
            if (_orders.TryGetValue(Tick, out var orders))
            {
                foreach (var order in orders)
                {
                    order.Apply(Battle, _arsenal);
                }
                _orders.Remove(Tick);
            }
            Battle.Tick(Step);
            Tick++;
            return true;
        }

        public long Checksum()
        {
            var sum = Mix(17L, Tick);
            sum = Mix(sum, Battle.Lives.Left);
            for (var player = 0; player < _treasury.Players; player++)
            {
                sum = Mix(sum, _treasury.Wallet(player).Gold);
            }
            foreach (var tower in Battle.Garrison.Towers)
            {
                sum = Mix(Mix(sum, tower.Id), tower.Level);
            }
            foreach (var monster in Battle.Arena.Horde.Monsters)
            {
                sum = Mix(Mix(sum, Rounded(monster.Distance)), Rounded(monster.Vitality));
            }
            return Mix(sum, Battle.Volley.Missiles.Count);
        }

        private static long Mix(long sum, long value) => (sum ^ value) * Prime;

        private static long Rounded(float value) => (long)Math.Round(value * Precision);
    }
}

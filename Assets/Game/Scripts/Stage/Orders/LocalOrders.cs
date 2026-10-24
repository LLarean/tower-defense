using System.Numerics;
using Game.Core.Battles;
using Game.Core.Towers;

namespace Game.Stage.Orders
{
    public sealed class LocalOrders : IOrders
    {
        private readonly Battle _battle;
        private readonly int _player;

        public LocalOrders(Battle battle, int player)
        {
            _battle = battle;
            _player = player;
        }

        public void Build(TowerSpec spec, Vector3 point) => _battle.Garrison.TryBuild(_player, spec, point, out _);

        public void Upgrade(Tower tower) => _battle.Garrison.TryUpgrade(_player, tower);

        public void Sell(Tower tower) => _battle.Garrison.TrySell(_player, tower);

        public void Summon() => _battle.Summon();
    }
}

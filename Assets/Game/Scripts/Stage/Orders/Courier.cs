using System.Collections.Generic;
using System.Numerics;
using Game.Core.Battles;
using Game.Core.Towers;

namespace Game.Stage.Orders
{
    public sealed class Courier : IOrders
    {
        private readonly IPost _post;
        private readonly IReadOnlyList<TowerSpec> _arsenal;
        private readonly int _player;

        public Courier(IPost post, IReadOnlyList<TowerSpec> arsenal, int player)
        {
            _post = post;
            _arsenal = arsenal;
            _player = player;
        }

        public void Build(TowerSpec spec, Vector3 point)
        {
            for (var index = 0; index < _arsenal.Count; index++)
            {
                if (_arsenal[index] == spec)
                {
                    _post.Send(Order.Build(_player, index, point));
                }
            }
        }

        public void Upgrade(Tower tower) => _post.Send(Order.Upgrade(_player, tower.Id));

        public void Sell(Tower tower) => _post.Send(Order.Sell(_player, tower.Id));

        public void Summon() => _post.Send(Order.Summon(_player));
    }
}

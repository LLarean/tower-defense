using System.Numerics;
using Game.Core.Towers;

namespace Game.Stage.Orders
{
    public interface IOrders
    {
        void Build(TowerSpec spec, Vector3 point);

        void Upgrade(Tower tower);

        void Sell(Tower tower);

        void Summon();
    }
}

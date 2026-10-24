using Game.Core.Battles;

namespace Game.Stage.Orders
{
    public interface IPost
    {
        void Send(Order order);
    }
}

using Game.Core.Battles;

namespace Game.Stage.Orders
{
    public sealed class LocalPost : IPost
    {
        private readonly Timeline _timeline;

        public LocalPost(Timeline timeline)
        {
            _timeline = timeline;
        }

        public void Send(Order order) => _timeline.Schedule(_timeline.Tick, order);
    }
}

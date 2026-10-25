namespace Game.Core.Towers
{
    public sealed class GarrisonFeedbacks : IGarrisonFeedback
    {
        private readonly IGarrisonFeedback[] _listeners;

        public GarrisonFeedbacks(params IGarrisonFeedback[] listeners)
        {
            _listeners = listeners;
        }

        public void Built(Tower tower)
        {
            foreach (var listener in _listeners)
            {
                listener.Built(tower);
            }
        }

        public void Upgraded(Tower tower)
        {
            foreach (var listener in _listeners)
            {
                listener.Upgraded(tower);
            }
        }

        public void Sold(Tower tower)
        {
            foreach (var listener in _listeners)
            {
                listener.Sold(tower);
            }
        }
    }
}

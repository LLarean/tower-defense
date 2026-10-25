using Game.Core.Towers;

namespace Game.Core.Tests
{
    public sealed class FakeGarrisonFeedback : IGarrisonFeedback
    {
        public int Built { get; private set; }

        public int Upgraded { get; private set; }

        public int Sold { get; private set; }

        void IGarrisonFeedback.Built(Tower tower) => Built++;

        void IGarrisonFeedback.Upgraded(Tower tower) => Upgraded++;

        void IGarrisonFeedback.Sold(Tower tower) => Sold++;
    }
}

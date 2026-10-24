namespace Game.Core.Towers
{
    public interface IGarrisonFeedback
    {
        void Built(Tower tower);

        void Upgraded(Tower tower);

        void Sold(Tower tower);
    }
}

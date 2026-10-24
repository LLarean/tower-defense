namespace Game.Core.Economy
{
    public sealed class Wallet
    {
        public Wallet(int gold)
        {
            Gold = gold;
        }

        public int Gold { get; private set; }

        public void Earn(int amount) => Gold += amount;

        public bool TrySpend(int amount)
        {
            if (amount > Gold)
            {
                return false;
            }
            Gold -= amount;
            return true;
        }
    }
}

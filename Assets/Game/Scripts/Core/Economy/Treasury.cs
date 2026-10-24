namespace Game.Core.Economy
{
    public sealed class Treasury
    {
        private readonly Wallet[] _wallets;

        public Treasury(int players, int gold)
        {
            _wallets = new Wallet[players];
            for (var player = 0; player < players; player++)
            {
                _wallets[player] = new Wallet(gold);
            }
        }

        public int Players => _wallets.Length;

        public Wallet Wallet(int player) => _wallets[player];

        public void Share(int bounty)
        {
            var portion = (bounty + _wallets.Length - 1) / _wallets.Length;
            foreach (var wallet in _wallets)
            {
                wallet.Earn(portion);
            }
        }
    }
}

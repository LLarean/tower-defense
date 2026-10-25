using Game.Core.Economy;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Purse : IWidget
    {
        private readonly Wallet _wallet;
        private readonly Lives _lives;
        private readonly Label _gold;
        private readonly Label _hearts;
        private int _shownGold = -1;
        private int _shownLives = -1;

        public Purse(VisualElement root, Wallet wallet, Lives lives)
        {
            _wallet = wallet;
            _lives = lives;
            _gold = root.Q<Label>("gold");
            _hearts = root.Q<Label>("lives");
        }

        public void Refresh()
        {
            if (_wallet.Gold != _shownGold)
            {
                _shownGold = _wallet.Gold;
                _gold.text = $"{_shownGold}";
            }
            if (_lives.Left != _shownLives)
            {
                _shownLives = _lives.Left;
                _hearts.text = $"{_shownLives}";
            }
        }
    }
}

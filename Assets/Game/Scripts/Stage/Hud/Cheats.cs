using Game.Core.Battles;
using Game.Core.Economy;
using Game.Stage.Orders;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Cheats
    {
        private const int Gold = 1000;
        private const int Hearts = 10;

        private readonly Battle _battle;

        public Cheats(VisualElement root, Battle battle, Wallet wallet, IOrders orders)
        {
            _battle = battle;
            root.Q<Button>("cheat-gold").clicked += () => wallet.Earn(Gold);
            root.Q<Button>("cheat-lives").clicked += () => battle.Lives.Gain(Hearts);
            root.Q<Button>("cheat-wave").clicked += orders.Summon;
            root.Q<Button>("cheat-kill").clicked += Massacre;
        }

        private void Massacre()
        {
            foreach (var monster in _battle.Arena.Horde.Monsters)
            {
                monster.Slay();
            }
        }
    }
}

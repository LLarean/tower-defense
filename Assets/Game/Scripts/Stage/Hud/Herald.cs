using System;
using Game.Core.Battles;
using Game.Stage.Orders;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Herald : IWidget
    {
        private readonly Battle _battle;
        private readonly Label _wave;
        private readonly Button _summon;
        private int _shownWave = -1;
        private int _shownCountdown = -1;

        public Herald(VisualElement root, Battle battle, IOrders orders)
        {
            _battle = battle;
            _wave = root.Q<Label>("wave");
            _summon = root.Q<Button>("summon");
            _summon.clicked += orders.Summon;
        }

        public void Refresh()
        {
            var invasion = _battle.Invasion;
            var countdown = (int)Math.Ceiling(Math.Max(0f, invasion.Countdown));
            if (invasion.Wave == _shownWave && countdown == _shownCountdown)
            {
                return;
            }
            _shownWave = invasion.Wave;
            _shownCountdown = countdown;
            _wave.text = $"Wave {invasion.Wave}/{invasion.Waves}";
            _summon.text = !_battle.Started ? "Start" : $"Next wave {countdown}s";
            _summon.EnableInClassList("hidden", invasion.Wave >= invasion.Waves);
        }
    }
}

using System;
using Game.Core.Battles;
using Game.Stage.Net;
using Game.Stage.Orders;
using Game.Stage.Speech;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Herald : IWidget
    {
        private readonly Battle _battle;
        private readonly IPace _pace;
        private readonly Lexicon _lexicon;
        private readonly Label _wave;
        private readonly Button _summon;
        private int _shownWave = -1;
        private int _shownCountdown = -1;
        private bool _shownDesync;

        public Herald(VisualElement root, Battle battle, IOrders orders, IPace pace, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _battle = battle;
            _pace = pace;
            _wave = root.Q<Label>("wave");
            _summon = root.Q<Button>("summon");
            _summon.clicked += orders.Summon;
        }

        public void Refresh()
        {
            var invasion = _battle.Invasion;
            var countdown = (int)Math.Ceiling(Math.Max(0f, invasion.Countdown));
            if (invasion.Wave == _shownWave && countdown == _shownCountdown && _pace.Desynced == _shownDesync)
            {
                return;
            }
            _shownWave = invasion.Wave;
            _shownCountdown = countdown;
            _shownDesync = _pace.Desynced;
            _wave.text = _lexicon.Say(_shownDesync ? "Wave {0}/{1} — out of sync!" : "Wave {0}/{1}", invasion.Wave, invasion.Waves);
            _summon.text = !_battle.Started ? _lexicon.Say("Start") : _lexicon.Say("Next wave {0}s", countdown);
            _summon.EnableInClassList("hidden", invasion.Wave >= invasion.Waves);
        }
    }
}

using Game.Core.Arenas;
using Game.Stage.Speech;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Bossbar : IWidget
    {
        private const float Step = 0.002f;

        private readonly Horde _horde;
        private readonly Bestiary _bestiary;
        private readonly VisualElement _bar;
        private readonly Label _title;
        private readonly VisualElement _fill;
        private readonly Lexicon _lexicon;
        private float _shown = -1f;

        public Bossbar(VisualElement root, Horde horde, Bestiary bestiary, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _horde = horde;
            _bestiary = bestiary;
            _bar = root.Q("bossbar");
            _title = root.Q<Label>("boss-title");
            _fill = root.Q("boss-fill");
            _bar.AddToClassList("hidden");
        }

        public void Refresh()
        {
            foreach (var monster in _horde.Monsters)
            {
                if (_bestiary.TryBoss(monster.Breed, out var title))
                {
                    Show(title, monster.Vitality);
                    return;
                }
            }
            if (_shown >= 0f)
            {
                _shown = -1f;
                _bar.AddToClassList("hidden");
            }
        }

        private void Show(string title, float vitality)
        {
            if (_shown < 0f)
            {
                _title.text = _lexicon.Say(title);
                _bar.RemoveFromClassList("hidden");
            }
            if (System.Math.Abs(vitality - _shown) < Step)
            {
                return;
            }
            _shown = vitality;
            _fill.style.width = Length.Percent(vitality * 100f);
        }
    }
}

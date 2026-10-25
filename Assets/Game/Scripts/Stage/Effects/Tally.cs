using System.Collections.Generic;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Stage.Config;
using Game.Stage.Hud;
using UnityEngine;

namespace Game.Stage.Effects
{
    public sealed class Tally
    {
        private const int Cached = 10000;
        private const string Light = "caption--number";
        private const string Heavy = "caption--heavy";

        private static readonly Vector3 Lift = new Vector3(0f, 2f, 0f);

        private readonly SpectacleAsset _spectacle;
        private readonly Captions _captions;
        private readonly Setting _setting;
        private readonly Numerals _numerals = new Numerals(Cached);
        private readonly Dictionary<Monster, int> _slots = new Dictionary<Monster, int>();
        private readonly List<(Monster Monster, float Amount, float Strongest, Element Element)> _scores =
            new List<(Monster, float, float, Element)>();
        private float _elapsed;

        public Tally(SpectacleAsset spectacle, Captions captions, Setting setting)
        {
            _spectacle = spectacle;
            _captions = captions;
            _setting = setting;
        }

        public void Add(Monster monster, float amount, Element element)
        {
            if (!_setting.On)
            {
                return;
            }
            if (!_slots.TryGetValue(monster, out var slot))
            {
                slot = _scores.Count;
                _slots.Add(monster, slot);
                _scores.Add((monster, 0f, 0f, element));
            }
            var score = _scores[slot];
            var strongest = amount > score.Strongest;
            _scores[slot] = (monster, score.Amount + amount, strongest ? amount : score.Strongest, strongest ? element : score.Element);
        }

        public void Tick(float seconds)
        {
            _elapsed += seconds;
            if (_elapsed >= _spectacle.Tallying)
            {
                _elapsed = 0f;
                Settle();
            }
            _captions.Tick(seconds);
        }

        private void Settle()
        {
            foreach (var score in _scores)
            {
                if (score.Amount >= _spectacle.Noticeable)
                {
                    var look = score.Amount >= _spectacle.Heavy ? Heavy : Light;
                    var text = _numerals.Of(Mathf.RoundToInt(score.Amount));
                    _captions.Say(text, _spectacle.Color(score.Element), score.Monster.Position.ToUnity() + Lift, look);
                }
            }
            _scores.Clear();
            _slots.Clear();
        }
    }
}

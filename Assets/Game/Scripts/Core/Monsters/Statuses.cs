using System;
using Game.Core.Elements;

namespace Game.Core.Monsters
{
    public sealed class Statuses
    {
        private const int PoisonStacks = 5;
        private const float SlowestPace = 0.1f;

        private static readonly Condition[] Conditions = (Condition[])Enum.GetValues(typeof(Condition));
        private static readonly Condition[] Marks =
        {
            Condition.Burning,
            Condition.Wet,
            Condition.Chilled,
            Condition.Charged,
            Condition.Poisoned,
            Condition.Sundered,
            Condition.Oiled,
        };

        private readonly float[] _remaining = new float[Conditions.Length];
        private readonly float[] _potency = new float[Conditions.Length];
        private float _stun;

        public static Condition[] All => Conditions;

        public static Condition MarkOf(Element element) => Marks[(int)element];

        public float Pace
        {
            get
            {
                if (_stun > 0f || Has(Condition.Frozen))
                {
                    return 0f;
                }
                var slow = Math.Min(1f, Potency(Condition.Chilled) + Potency(Condition.Mired) + Potency(Condition.Oiled));
                return Math.Max(SlowestPace, 1f - slow);
            }
        }

        public bool Has(Condition condition) => _remaining[(int)condition] > 0f;

        public float Potency(Condition condition) => Has(condition) ? _potency[(int)condition] : 0f;

        public float Remaining(Condition condition) => _remaining[(int)condition];

        public void Inflict(Condition condition, float potency, float duration)
        {
            var index = (int)condition;
            if (condition == Condition.Poisoned)
            {
                _potency[index] = Math.Min(Potency(condition) + potency, potency * PoisonStacks);
            }
            else
            {
                _potency[index] = Math.Max(Potency(condition), potency);
            }
            _remaining[index] = Math.Max(_remaining[index], duration);
            if (condition == Condition.Charged)
            {
                _stun = Math.Max(_stun, potency);
            }
        }

        public void Cure(Condition condition)
        {
            _remaining[(int)condition] = 0f;
            _potency[(int)condition] = 0f;
        }

        public void Tick(float seconds)
        {
            for (var index = 0; index < _remaining.Length; index++)
            {
                _remaining[index] = Math.Max(0f, _remaining[index] - seconds);
            }
            _stun = Math.Max(0f, _stun - seconds);
        }
    }
}

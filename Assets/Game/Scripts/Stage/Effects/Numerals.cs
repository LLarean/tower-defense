using System.Globalization;

namespace Game.Stage.Effects
{
    public sealed class Numerals
    {
        private readonly string[] _cache;

        public Numerals(int count)
        {
            _cache = new string[count];
        }

        public string Of(int number)
        {
            if (number < 0 || number >= _cache.Length)
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
            return _cache[number] ??= number.ToString(CultureInfo.InvariantCulture);
        }
    }
}

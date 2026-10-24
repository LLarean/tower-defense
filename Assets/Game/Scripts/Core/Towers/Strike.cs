using System.Collections.Generic;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Towers
{
    public sealed class Strike
    {
        private readonly IReadOnlyList<Element> _elements;
        private readonly Grade _grade;

        public Strike(IReadOnlyList<Element> elements, Grade grade)
        {
            _elements = elements;
            _grade = grade;
        }

        public void Land(Monster target, Arena arena)
        {
            var share = _grade.Damage / _elements.Count;
            foreach (var element in _elements)
            {
                target.Take(new Hit(element, share, _grade.Potency, _grade.Duration), arena);
            }
        }
    }
}

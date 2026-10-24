using System.Collections.Generic;
using Game.Core.Elements;

namespace Game.Core.Towers
{
    public sealed class TowerSpec
    {
        private readonly Element[] _elements;
        private readonly Grade[] _grades;

        public TowerSpec(string name, float speed, Element[] elements, params Grade[] grades)
        {
            Name = name;
            Speed = speed;
            _elements = elements;
            _grades = grades;
        }

        public string Name { get; }

        public float Speed { get; }

        public IReadOnlyList<Element> Elements => _elements;

        public int Levels => _grades.Length;

        public int Price => _grades[0].Cost;

        public Grade Grade(int level) => _grades[level];
    }
}

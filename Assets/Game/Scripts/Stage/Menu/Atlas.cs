using System.Collections.Generic;

namespace Game.Stage.Menu
{
    public sealed class Atlas
    {
        private readonly IReadOnlyList<string> _maps;
        private int _current;

        public Atlas(IReadOnlyList<string> maps)
        {
            _maps = maps;
        }

        public string Current => _maps[_current];

        public void Next() => _current = (_current + 1) % _maps.Count;
    }
}

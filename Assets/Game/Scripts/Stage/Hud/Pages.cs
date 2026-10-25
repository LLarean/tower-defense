using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Pages
    {
        private readonly List<VisualElement> _pages;

        public Pages(VisualElement container)
        {
            _pages = container.Query(className: "page").ToList();
            Current = string.Empty;
        }

        public string Current { get; private set; }

        public void Open(string name)
        {
            Current = name;
            foreach (var page in _pages)
            {
                page.EnableInClassList("hidden", page.name != name);
            }
        }
    }
}

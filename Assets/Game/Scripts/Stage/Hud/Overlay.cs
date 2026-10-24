using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Overlay : ICover
    {
        private readonly VisualElement _root;

        public Overlay(VisualElement root)
        {
            _root = root;
        }

        public bool Covers(Vector2 pointer)
        {
            var panel = _root.panel;
            var point = RuntimePanelUtils.ScreenToPanel(panel, new Vector2(pointer.x, Screen.height - pointer.y));
            return panel.Pick(point) != null;
        }
    }
}

using Game.Stage.Config;
using UnityEngine.UIElements;

namespace Game.Stage.Sound
{
    public sealed class Chime
    {
        public Chime(VisualElement root, Voices voices, CueData click)
        {
            root.RegisterCallback<ClickEvent>(press =>
            {
                if (press.target is Button)
                {
                    voices.Play(click);
                }
            }, TrickleDown.TrickleDown);
        }
    }
}

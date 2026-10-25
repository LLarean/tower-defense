using System;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Prompt
    {
        private const string Page = "pause-prompt";

        private readonly Pages _pages;
        private readonly Label _question;
        private Action _action = () => { };

        public Prompt(VisualElement root, Pages pages)
        {
            _pages = pages;
            _question = root.Q<Label>("prompt-question");
            root.Q<Button>("prompt-yes").clicked += () => _action();
        }

        public Action Ask(string question, Action action) => () =>
        {
            _question.text = question;
            _action = action;
            _pages.Open(Page);
        };
    }
}

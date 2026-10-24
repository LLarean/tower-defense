using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Pause
    {
        private readonly VisualElement _screen;
        private readonly bool _online;

        public Pause(VisualElement root, Codex codex, bool online, Action menu)
        {
            _screen = root.Q("pause");
            _online = online;
            root.Q<Button>("resume").clicked += Toggle;
            var restart = root.Q<Button>("restart");
            restart.EnableInClassList("hidden", online);
            restart.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            root.Q<Button>("codex").clicked += codex.Open;
            root.Q<Button>("menu").clicked += menu;
            root.Q<Button>("quit").clicked += Application.Quit;
            _screen.AddToClassList("hidden");
            Time.timeScale = 1f;
        }

        public void Toggle()
        {
            var open = _screen.ClassListContains("hidden");
            _screen.EnableInClassList("hidden", !open);
            if (!_online)
            {
                Time.timeScale = open ? 0f : 1f;
            }
        }
    }
}

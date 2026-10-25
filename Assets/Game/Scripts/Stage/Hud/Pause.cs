using System;
using Game.Stage.Speech;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage.Hud
{
    public sealed class Pause
    {
        private const string Main = "pause-main";

        private readonly VisualElement _screen;
        private readonly Pages _pages;
        private readonly Tempo _tempo;
        private readonly bool _online;

        public Pause(VisualElement root, Tempo tempo, bool online, Action menu, Lexicon lexicon)
        {
            _screen = root.Q("pause");
            _pages = new Pages(_screen);
            _tempo = tempo;
            _online = online;
            var prompt = new Prompt(root, _pages);
            root.Q<Button>("open-menu").clicked += Toggle;
            root.Q<Button>("resume").clicked += Toggle;
            root.Q<Button>("restart").clicked += prompt.Ask(lexicon.Say("Restart the mission?"), Restart);
            Navigate(root, "settings", "pause-settings");
            Navigate(root, "controls", "pause-controls");
            Navigate(root, "cheats", "pause-cheats");
            root.Q<Button>("restart").EnableInClassList("hidden", online);
            root.Q<Button>("cheats").EnableInClassList("hidden", online);
            root.Q<Button>("menu").clicked += prompt.Ask(lexicon.Say("Leave to the main menu?"), menu);
            root.Q<Button>("quit").clicked += prompt.Ask(lexicon.Say("Quit the game?"), Application.Quit);
            _screen.Query<Button>(className: "back").ForEach(back => back.clicked += () => _pages.Open(Main));
            _screen.AddToClassList("hidden");
        }

        public void Escape()
        {
            if (Opened && _pages.Current != Main)
            {
                _pages.Open(Main);
                return;
            }
            Toggle();
        }

        public void Toggle()
        {
            var open = !Opened;
            _screen.EnableInClassList("hidden", !open);
            _pages.Open(Main);
            if (!_online)
            {
                _tempo.Halt(open);
            }
        }

        private bool Opened => !_screen.ClassListContains("hidden");

        private void Navigate(VisualElement root, string name, string page) => root.Q<Button>(name).clicked += () => _pages.Open(page);

        private static void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

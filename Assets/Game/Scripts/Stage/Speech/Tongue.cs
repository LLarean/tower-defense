using Game.Stage.Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage.Speech
{
    public sealed class Tongue
    {
        private const string Key = "language";

        private readonly LanguageAsset _languages;
        private readonly int _current;

        public Tongue(LanguageAsset languages)
        {
            _languages = languages;
            _current = Mathf.Clamp(PlayerPrefs.GetInt(Key, languages.Native(Application.systemLanguage)), 0, languages.Count - 1);
            Lexicon = new Lexicon(languages.Table(_current));
        }

        public Lexicon Lexicon { get; }

        public string Title => _languages.Title(_current);

        public void Dress(VisualElement root)
        {
            var look = _languages.Look(_current);
            if (look.Length > 0)
            {
                root.AddToClassList(look);
            }
            Lexicon.Translate(root);
        }

        public void Switch()
        {
            PlayerPrefs.SetInt(Key, (_current + 1) % _languages.Count);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

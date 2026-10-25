using System;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Languages")]
    public sealed class LanguageAsset : ScriptableObject
    {
        [Serializable]
        private struct Language
        {
            [SerializeField] private string _title;
            [SerializeField] private SystemLanguage _system;
            [SerializeField] private TextAsset _table;
            [SerializeField] private string _look;

            public string Title => _title;

            public SystemLanguage System => _system;

            public string Table => _table != null ? _table.text : string.Empty;

            public string Look => _look;
        }

        [SerializeField] private Language[] _languages = new Language[0];

        public int Count => _languages.Length;

        public string Title(int language) => _languages[language].Title;

        public string Table(int language) => _languages[language].Table;

        public string Look(int language) => _languages[language].Look;

        public int Native(SystemLanguage system)
        {
            for (var language = 0; language < _languages.Length; language++)
            {
                if (_languages[language].System == system)
                {
                    return language;
                }
            }
            return 0;
        }
    }
}

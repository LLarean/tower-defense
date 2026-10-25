using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.Stage.Speech
{
    public sealed class Lexicon
    {
        private readonly Dictionary<string, string> _words = new Dictionary<string, string>();

        public Lexicon(string table)
        {
            foreach (var line in table.Split('\n'))
            {
                var tab = line.IndexOf('\t');
                if (tab > 0)
                {
                    _words[Unescape(line.Substring(0, tab))] = Unescape(line.Substring(tab + 1).TrimEnd('\r'));
                }
            }
        }

        public string Say(string english) => english.Length > 0 && _words.TryGetValue(english, out var word) ? word : english;

        public string Say(string english, object first) => string.Format(Say(english), first);

        public string Say(string english, object first, object second) => string.Format(Say(english), first, second);

        public void Translate(VisualElement root)
        {
            root.Query<TextElement>().ForEach(element => element.text = Say(element.text));
            root.Query<TextField>().ForEach(field => field.textEdition.placeholder = Say(field.textEdition.placeholder));
        }

        private static string Unescape(string text) => text.Replace("\\n", "\n").Replace("\\t", "\t");
    }
}

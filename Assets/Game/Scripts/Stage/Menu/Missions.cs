using System.Collections.Generic;
using Game.Stage.Config;
using Game.Stage.Speech;
using Game.Stage.Story;
using UnityEngine.UIElements;

namespace Game.Stage.Menu
{
    public sealed class Missions
    {
        private readonly CampaignAsset _campaign;
        private readonly DifficultyAsset _difficulty;
        private readonly Sortie _sortie;
        private readonly Lexicon _lexicon;
        private readonly List<Button> _entries = new List<Button>();
        private readonly List<Button> _tiers = new List<Button>();
        private readonly Label _title;
        private readonly Label _briefing;
        private readonly Label _summary;

        public Missions(VisualElement root, CampaignAsset campaign, DifficultyAsset difficulty, Sortie sortie, Chronicle chronicle, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _campaign = campaign;
            _difficulty = difficulty;
            _sortie = sortie;
            _title = root.Q<Label>("mission-title");
            _briefing = root.Q<Label>("mission-briefing");
            _summary = root.Q<Label>("tier-summary");
            Fill(root.Q("mission-list"), chronicle);
            Offer(root.Q("tiers"));
            Show();
        }

        private void Fill(VisualElement list, Chronicle chronicle)
        {
            for (var index = 0; index < _campaign.Missions.Count; index++)
            {
                var mission = index;
                var level = _campaign.Missions[index];
                var entry = new Button(() => Select(mission)) { text = $"{index + 1}. {_lexicon.Say(level.Title)}" };
                entry.AddToClassList("mission");
                entry.EnableInClassList("mission--done", chronicle.Completed(level));
                var unlocked = chronicle.Unlocked(_campaign, index);
                entry.EnableInClassList("mission--locked", !unlocked);
                entry.SetEnabled(unlocked);
                list.Add(entry);
                _entries.Add(entry);
            }
        }

        private void Offer(VisualElement row)
        {
            for (var index = 0; index < _difficulty.Count; index++)
            {
                var tier = index;
                var button = new Button(() => Pick(tier)) { text = _lexicon.Say(_difficulty.Title(index)) };
                button.AddToClassList("choice");
                button.AddToClassList("seal");
                button.AddToClassList("grow");
                row.Add(button);
                _tiers.Add(button);
            }
        }

        private void Select(int mission)
        {
            _sortie.Choose(mission);
            Show();
        }

        private void Pick(int tier)
        {
            _sortie.Pick(tier);
            Show();
        }

        private void Show()
        {
            for (var index = 0; index < _entries.Count; index++)
            {
                _entries[index].EnableInClassList("mission--chosen", index == _sortie.Mission);
            }
            for (var index = 0; index < _tiers.Count; index++)
            {
                _tiers[index].EnableInClassList("seal--quiet", index != _sortie.Difficulty);
            }
            _title.text = _lexicon.Say(_sortie.Level.Title);
            _briefing.text = _lexicon.Say(_sortie.Level.Briefing);
            _summary.text = _lexicon.Say(_difficulty.Summary(_sortie.Difficulty));
        }
    }
}

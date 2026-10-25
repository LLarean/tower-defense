using Game.Stage.Config;
using Game.Stage.Net;
using Game.Stage.Speech;
using Game.Stage.Story;
using UnityEngine;

namespace Game.Stage.Menu
{
    public sealed class Sortie
    {
        private const string Preferred = "difficulty";

        private readonly CampaignAsset _campaign;
        private readonly DifficultyAsset _difficulty;
        private readonly Match _match;

        public Sortie(CampaignAsset campaign, DifficultyAsset difficulty, Match match, Chronicle chronicle)
        {
            _campaign = campaign;
            _difficulty = difficulty;
            _match = match;
            Mission = Frontier(campaign, chronicle);
            Pick(PlayerPrefs.GetInt(Preferred, difficulty.Default));
        }

        public int Mission { get; private set; }

        public int Difficulty { get; private set; }

        public LevelAsset Level => _campaign.Missions[Mission];

        public string Summary(Lexicon lexicon) => $"{lexicon.Say(Level.Title)} · {lexicon.Say(_difficulty.Title(Difficulty))}";

        public void Choose(int mission) => Mission = mission;

        public void Pick(int difficulty)
        {
            Difficulty = Mathf.Clamp(difficulty, 0, _difficulty.Count - 1);
            PlayerPrefs.SetInt(Preferred, Difficulty);
            _match.Choose(Difficulty);
        }

        private static int Frontier(CampaignAsset campaign, Chronicle chronicle)
        {
            var mission = 0;
            while (mission + 1 < campaign.Missions.Count && chronicle.Completed(campaign.Missions[mission]))
            {
                mission++;
            }
            return mission;
        }
    }
}

using Game.Stage.Config;
using UnityEngine;

namespace Game.Stage.Story
{
    public sealed class Chronicle
    {
        private const string Done = "campaign.done.";
        private const string Heard = "campaign.prologue";

        public bool Prologued => PlayerPrefs.GetInt(Heard, 0) == 1;

        public bool Completed(LevelAsset level) => PlayerPrefs.GetInt(Done + level.name, 0) == 1;

        public bool Unlocked(CampaignAsset campaign, int mission) => mission == 0 || Completed(campaign.Missions[mission - 1]);

        public void Record(LevelAsset level)
        {
            PlayerPrefs.SetInt(Done + level.name, 1);
            PlayerPrefs.Save();
        }

        public void Hear()
        {
            PlayerPrefs.SetInt(Heard, 1);
            PlayerPrefs.Save();
        }
    }
}

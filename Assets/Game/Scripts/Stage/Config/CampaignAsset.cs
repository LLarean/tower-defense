using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Campaign")]
    public sealed class CampaignAsset : ScriptableObject
    {
        [SerializeField] private string _title = "Campaign";
        [SerializeField, TextArea(5, 12)] private string _prologue = string.Empty;
        [SerializeField, TextArea(2, 6)] private string _defeat = string.Empty;
        [SerializeField] private LevelAsset[] _missions = new LevelAsset[0];

        public string Title => _title;

        public string Prologue => _prologue;

        public string Defeat => _defeat;

        public IReadOnlyList<LevelAsset> Missions => _missions;

        public bool TryNext(LevelAsset level, out LevelAsset next)
        {
            var index = System.Array.IndexOf(_missions, level);
            var found = index >= 0 && index + 1 < _missions.Length;
            next = found ? _missions[index + 1] : level;
            return found;
        }
    }
}

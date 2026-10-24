using UnityEngine;

namespace Game
{
    public class MatchSettings : MonoBehaviour
    {
        [SerializeField] private MatchModel _matchModel;

        private int _currentRoundIndex = -1;

        public float RoundStartDelay => _matchModel.RoundStartDelay;

        public bool TryGetCurrentRoundModel(out RoundModel currentRoundModel)
        {
            var isSuccess = false;
            currentRoundModel = new RoundModel();

            if (_currentRoundIndex < _matchModel.RoundSettings.Count)
            {
                currentRoundModel = _matchModel.RoundSettings[_currentRoundIndex];
                isSuccess = true;
            }

            return isSuccess;
        }
        
        public bool TryGetNextRoundModel(out RoundModel nextRoundModel)
        {
            var isSuccess = false;
            nextRoundModel = new RoundModel();
            var nextRoundIndex = _currentRoundIndex + 1;
            
            if (nextRoundIndex < _matchModel.RoundSettings.Count)
            {
                nextRoundModel = _matchModel.RoundSettings[nextRoundIndex];
                _currentRoundIndex = nextRoundIndex;
                isSuccess = true;
            }

            return isSuccess;
        }

        public void ResetCurrentRoundIndex() => _currentRoundIndex = -1;
    }
}
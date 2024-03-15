using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private Button _menu;
        [SerializeField] private GameClock _gameClock;
        [SerializeField] private TMP_Text _gold;

        public event Action OnMenuClicked;
    
        public void StartClock() => _gameClock.StartCounting();

        public void PauseClock() => _gameClock.PauseCounting();
    
        public void ResetClock() => _gameClock.ResetCounting();

        public void ShowGold(int value)
        {
            _gold.text = $"{GlobalStrings.Gold}: {value.ToString()}";
        }

        private void Start()
        {
            _menu.onClick.AddListener(OnMenuClick);
        }

        private void OnMenuClick() => OnMenuClicked?.Invoke();
    }
}
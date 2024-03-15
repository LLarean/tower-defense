using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameClock : MonoBehaviour
    {
        [SerializeField] private TMP_Text _time;
    
        private bool _isCounting;
        private float _currentTime;
        
        private float _duration = 1f;

        public void StartCounting()
        {
            _isCounting = true;
            StartCoroutine(Counting());
        }

        public void PauseCounting()
        {
            _isCounting = false;
        }

        public void ResetCounting()
        {
            _isCounting = false;
            _currentTime = 0;
            UpdateTimerDisplay();
        }

        private IEnumerator Counting()
        {
            while (_isCounting == true)
            {
                _currentTime++;
                UpdateTimerDisplay();
                yield return new WaitForSeconds(_duration);
            }
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(_currentTime / 60);
            int seconds = Mathf.FloorToInt(_currentTime % 60);

            _time.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
using System.Collections;
using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    [SerializeField] private TMP_Text _time;
    
    private bool _isCounting;
    private float _currentTime;

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
    }

    private IEnumerator Counting()
    {
        while (_isCounting == true)
        {
            _currentTime++;
            UpdateTimerDisplay();
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);

        _time.text = $"{minutes:00}:{seconds:00}";
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private Button _menu;
    [SerializeField] private GameClock _gameClock;
    [SerializeField] private TMP_Text _gold;

    private int _goldCount = 100;

    public void StartClock() => _gameClock.StartCounting();

    public void PauseClock() => _gameClock.PauseCounting();
    
    public void ResetCounting() => _gameClock.ResetCounting();

    public void AddGold()
    {
        _goldCount += 10;
        _gold.text = $"Gold: {_goldCount.ToString()}";
    }

    private void Start()
    {
        _gold.text = $"Gold: {_goldCount.ToString()}";
    }
}
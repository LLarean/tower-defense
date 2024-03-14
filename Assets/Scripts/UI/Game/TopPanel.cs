using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private Button _menu;
    [SerializeField] private GameClock _gameClock;
    [SerializeField] private TMP_Text _gold;

    public void StartClock() => _gameClock.StartCounting();

    public void PauseClock() => _gameClock.PauseCounting();
    
    public void ResetCounting() => _gameClock.ResetCounting();
}

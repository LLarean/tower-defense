using GameUtilities;
using Globals;
using ModalWindows;
using NaughtyAttributes;
using UI.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public class GameMediator : MonoBehaviour
    {
        [Inject] private Referee _referee;
        [Inject] private RoundStarter _roundStarter;
        [Inject] private MatchSettings _matchSettings;
        [Inject] private Builder _builder;
        [Inject] private HUD _hud;
        [Inject] private ConfirmationWindow _confirmationWindow;
        [Inject] private NotificationWindow _notificationWindow;

        [Button()] public void LoadMainMenu() => SceneManager.LoadScene(GlobalStrings.Menu);
        
        [Button()] public void StartMatch() => _referee.StartMatch();
        [Button()] public void StopMatch() => _roundStarter.StopMatch();
        [Button()] public void RestartRound() => _roundStarter.RestartRound();
        public bool TryStartRound() => _roundStarter.TryStartRound();
        
        [Button()] public void BuildFireTower() => _hud.BuildFireTower();
        [Button()] public void BuildAirTower() => _hud.BuildPoisonTower();
        [Button()] public void BuildWaterTower() => _hud.BuildWaterTower();
        [Button()] public void BuildIceTower() => _hud.BuildIceTower();
    
        [Button()] public void StartClock() => _hud.StartClock();
        [Button()] public void PauseClock() => _hud.PauseClock();
        [Button()] public void ResetClock() => _hud.ResetClock();
        [Button()] public void ClearInfo() => _hud.ClearInfo();
    
        [Button()] public void ShowConfirmationWindow() => _confirmationWindow.Show();
        [Button()] public void HideConfirmationWindow() => _confirmationWindow.Hide();
        
        [Button()] public void ShowNotificationWindow() => _notificationWindow.Show();
        [Button()] public void HideNotificationWindow() => _notificationWindow.Hide();

        public bool TryGetCurrentRoundModel(out RoundModel roundModel) => _matchSettings.TryGetCurrentRoundModel(out roundModel);
    }
}
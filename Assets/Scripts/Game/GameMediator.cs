using GameUtilities;
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
        [Inject] private RoundStarter _roundStarter;
        [Inject] private Builder _builder;
        [Inject] private HUD _hud;
        [Inject] private ConfirmationWindow _confirmationWindow;
        [Inject] private NotificationWindow _notificationWindow;
        [Inject] private AudioPlayer _audioPlayer;

        [Button()] public void LoadMainMenu() => SceneManager.LoadScene(GlobalStrings.Menu);
        
        [Button()] public void StartMatch() => _roundStarter.StartMatch();
        [Button()] public void StopMatch() => _roundStarter.StopMatch();
    
        [Button()] public void BuildFireTower() => _hud.BuildFireTower();
        [Button()] public void BuildAirTower() => _hud.BuildPoisonTower();
        [Button()] public void BuildWaterTower() => _hud.BuildWaterTower();
        [Button()] public void BuildIceTower() => _hud.BuildIceTower();
    
        [Button()] public void StartClock() => _hud.StartClock();
        [Button()] public void PauseClock() => _hud.PauseClock();
        [Button()] public void ResetClock() => _hud.ResetClock();
        [Button()] public void ClearInfo() => _hud.ClearInfo();
    
        public void InitializeConfirmationWindow(ConfirmationWindowModel confirmationWindowModel) => _confirmationWindow.Initialize(confirmationWindowModel);
        [Button()] public void ShowConfirmationWindow() => _confirmationWindow.Show();
        [Button()] public void HideConfirmationWindow() => _confirmationWindow.Hide();
        
        public void InitializeNotificationWindow(NotificationWindowModel notificationWindowModel) => _notificationWindow.Initialize(notificationWindowModel);
        [Button()] public void ShowNotificationWindow() => _notificationWindow.Show();
        [Button()] public void HideNotificationWindow() => _notificationWindow.Hide();
        
        [Button()] public void PlayBattleMusic() => _audioPlayer.PlayBattleMusic();
    }
}
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
        
        [Button()]
        public void RestartMatch()
        {
            _referee.ResetMatch();
            _roundStarter.ResetMatch();
            _builder.DestroyAllBuildings();
            _hud.Reset();
            
            HideConfirmationWindow();
            StartMatch();
        }

        [Button()] public void StopMatch() => _roundStarter.StopMatch();
        [Button()] public void RestartRound() => _roundStarter.RestartRound();
        public bool TryStartRound() => _roundStarter.TryStartRound();

        [Button()] public void ShowConfirmationWindow() => _confirmationWindow.Show();
        [Button()] public void HideConfirmationWindow() => _confirmationWindow.Hide();
        
        [Button()] public void ShowNotificationWindow() => _notificationWindow.Show();
        [Button()] public void HideNotificationWindow() => _notificationWindow.Hide();

        public bool TryGetCurrentRoundModel(out RoundModel roundModel) => _matchSettings.TryGetCurrentRoundModel(out roundModel);
    }
}
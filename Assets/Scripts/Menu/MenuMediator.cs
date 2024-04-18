using GameUtilities;
using ModalWindows;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Menu
{
    public class MenuMediator : MonoBehaviour
    {
        [Inject] private RoundStarter _roundStarter;
        [Inject] private ConfirmationWindow _confirmationWindow;
        [Inject] private SettingsWindow _settingsWindow;
        
        [Button()] public void LoadSingleplayerGame() => SceneManager.LoadScene(GlobalStrings.Demo);
        [Button()] public void LoadMultiplayerGame() => SceneManager.LoadScene(GlobalStrings.Demo);
        [Button()] public void ShowSettingsWindow() => _settingsWindow.Show();
        [Button()] public void CloseGame() => Application.Quit();

        public void InitializeConfirmationWindow(ConfirmationWindowModel confirmationWindowModel) => _confirmationWindow.Initialize(confirmationWindowModel);
        [Button()] public void ShowConfirmationWindow() => _confirmationWindow.Show();
        [Button()] public void HideConfirmationWindow() => _confirmationWindow.Hide();
    }
}
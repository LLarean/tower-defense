using Game;
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
        [Inject] private MenuReferee _menuReferee;
        [Inject] private RoundStarter _roundStarter;
        [Inject] private ConfirmationWindow _confirmationWindow;
        [Inject] private SettingsWindow _settingsWindow;
        
        [Button()] public void StartMatch() => _menuReferee.StartMatch();
        
        [Button()] public void StartRound() => _roundStarter.TryStartRound();
        [Button()] public void RestartRound() => _roundStarter.RestartRound();

        [Button()] public void LoadSingleplayerGame() => SceneManager.LoadScene(GlobalStrings.Demo);

        [Button()] public void LoadMultiplayerGame() => SceneManager.LoadScene(GlobalStrings.Demo);

        [Button()] public void ShowSettingsWindow() => _settingsWindow.Show();

        public void CloseGame() => Application.Quit();

        [Button()] public void ShowConfirmationWindow() => _confirmationWindow.Show();

        [Button()] public void HideConfirmationWindow() => _confirmationWindow.Hide();
    }
}
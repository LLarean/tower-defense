using Menu;
using ModalWindows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Button _singleplayer;
        [SerializeField] private Button _multiplayer;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;
        [SerializeField] private TMP_Text _version;

        private MenuMediator _menuMediator;

        [Inject]
        public void Construct(MenuMediator menuMediator)
        {
            _menuMediator = menuMediator;
        }
        
        private void Start()
        {
            _singleplayer.onClick.AddListener(LoadSingleplayerGame);
            _multiplayer.onClick.AddListener(LoadMultiplayerGame);
            _settings.onClick.AddListener(OpenSettings);
            _exit.onClick.AddListener(CloseGame);
            _version.text = Application.version;
        }

        private void LoadSingleplayerGame() => _menuMediator.LoadSingleplayerGame();
        
        private void LoadMultiplayerGame() => _menuMediator.LoadMultiplayerGame();
        
        private void OpenSettings() => _menuMediator.OpenSettings();

        private void CloseGame()
        {
            AcceptDelegate acceptDelegate = () => { _menuMediator.CloseGame(); };
            CancelDelegate cancelDelegate = () => { _menuMediator.HideConfirmationWindow(); };
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel
            {
                Title = "Exit",
                Message = "Are you sure you want to get out?",
                AcceptLabel = "Yes",
                CancelLabel = "No",
                AcceptDelegate = acceptDelegate,
                CancelDelegate = cancelDelegate,
            };
            
            _menuMediator.InitializeConfirmationWindow(confirmationWindowModel);
            _menuMediator.ShowConfirmationWindow();
        }
    }
}
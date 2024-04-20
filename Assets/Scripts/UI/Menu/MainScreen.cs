using DG.Tweening;
using Infrastructure;
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
        [SerializeField] private Transform _menuPanel;
        [Space]
        [SerializeField] private Button _singleplayer;
        [SerializeField] private Button _multiplayer;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;
        [SerializeField] private TMP_Text _version;

        private MenuMediator _menuMediator;
        private ConfirmationWindowModel _confirmationWindowModel;

        [Inject]
        public void Construct(MenuMediator menuMediator, ConfirmationWindowModel confirmationWindowModel)
        {
            _menuMediator = menuMediator;
            _confirmationWindowModel = confirmationWindowModel;
        }
        
        private void Start()
        {
            _singleplayer.onClick.AddListener(LoadSingleplayerGame);
            _multiplayer.onClick.AddListener(LoadMultiplayerGame);
            _settings.onClick.AddListener(OpenSettings);
            _exit.onClick.AddListener(CloseGame);
            _version.text = Application.version;

            ShowPanel();
        }

        private void LoadSingleplayerGame()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _menuMediator.LoadSingleplayerGame();
        }

        private void LoadMultiplayerGame()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
        }

        private void OpenSettings()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _menuMediator.ShowSettingsWindow();
        }

        private void CloseGame()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());

            SetConfirmationWindowModel();

            _menuMediator.ShowConfirmationWindow();
        }

        private void SetConfirmationWindowModel()
        {
            AcceptDelegate acceptDelegate = () => { _menuMediator.CloseGame(); };
            CancelDelegate cancelDelegate = () => { _menuMediator.HideConfirmationWindow(); };

            _confirmationWindowModel.Title = "Exit";
            _confirmationWindowModel.Message = "Are you sure you want to get out?";
            _confirmationWindowModel.AcceptLabel = "Yes";
            _confirmationWindowModel.CancelLabel = "No";
            _confirmationWindowModel.AcceptDelegate = acceptDelegate;
            _confirmationWindowModel.CancelDelegate = cancelDelegate;
        }

        private void ShowPanel()
        {
            var startPositionY = _menuPanel.position.y;
            _menuPanel.DOMoveY(startPositionY + 2000, 0);
            _menuPanel.DOMoveY(startPositionY, .7f);
        }
    }
}
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Button _start;
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
            _start.onClick.AddListener(LoadGameScene);
            _exit.onClick.AddListener(CloseGame);
            _version.text = Application.version;
        }

        private void LoadGameScene() => _menuMediator.LoadGameScene();

        private void CloseGame() => _menuMediator.CloseGame();
    }
}
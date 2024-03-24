using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _exit;

        private void Start()
        {
            _start.onClick.AddListener(LoadGameScene);
            _exit.onClick.AddListener(Exit);
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(GlobalStrings.Demo);
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}

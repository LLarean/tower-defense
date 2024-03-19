using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;

        private void Start()
        {
            _start.onClick.AddListener(LoadGameScene);
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(GlobalStrings.Demo);
        }
    }
}

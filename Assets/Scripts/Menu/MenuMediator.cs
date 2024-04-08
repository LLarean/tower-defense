using GameUtilities;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Menu
{
    public class MenuMediator : MonoBehaviour
    {
        [Inject] private RoundStarter _roundStarter;

        [Button()] public void StartMatch() => _roundStarter.StartMatch();
        
        [Button()] public void LoadGameScene() => SceneManager.LoadScene(GlobalStrings.Demo);
        [Button()] public void CloseGame() => Application.Quit();
    }
}
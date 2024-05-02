using Infrastructure;
using UnityEngine;
using Utilities;
using Zenject;

namespace Menu
{
    public class MenuEntryPoint : MonoBehaviour
    {
        [Inject] private MenuMediator _menuMediator;

        private void Start()
        {
            CustomLogger.Log("The menu scene is loaded", 2);
            
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleLoadMenuScene());
            
            _menuMediator.StartMatch();
        }
    }
}
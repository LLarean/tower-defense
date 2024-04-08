using UI.Menu;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainScreenInstaller : MonoInstaller
    {
        [SerializeField] private MainScreen _mainScreen;
    
        public override void InstallBindings()
        {
            Container
                .Bind<MainScreen>()
                .FromInstance(_mainScreen)
                .AsSingle();
        }
    }
}
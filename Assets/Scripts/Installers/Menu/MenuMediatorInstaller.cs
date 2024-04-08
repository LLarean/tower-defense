using Menu;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MenuMediatorInstaller : MonoInstaller
    {
        [SerializeField] private MenuMediator _menuMediator;
    
        public override void InstallBindings()
        {
            Container
                .Bind<MenuMediator>()
                .FromInstance(_menuMediator)
                .AsSingle();
        }
    }
}
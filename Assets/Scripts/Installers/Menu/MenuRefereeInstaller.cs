using Game;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MenuRefereeInstaller : MonoInstaller
    {
        [SerializeField] private MenuReferee _menuReferee;
    
        public override void InstallBindings()
        {
            Container
                .Bind<MenuReferee>()
                .FromInstance(_menuReferee)
                .AsSingle();
        }
    }
}
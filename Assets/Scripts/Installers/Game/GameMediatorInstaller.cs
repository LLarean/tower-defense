using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameMediatorInstaller : MonoInstaller
    {
        [SerializeField] private GameMediator _gameMediator;
    
        public override void InstallBindings()
        {
            Container
                .Bind<GameMediator>()
                .FromInstance(_gameMediator)
                .AsSingle();
        }
    }
}
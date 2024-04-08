using GameUtilities;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class RoundStarterInstaller : MonoInstaller
    {
        [SerializeField] private RoundStarter _roundStarter;
    
        public override void InstallBindings()
        {
            Container
                .Bind<RoundStarter>()
                .FromInstance(_roundStarter)
                .AsSingle();
        }
    }
}
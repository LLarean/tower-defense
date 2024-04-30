using Game;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class RefereeInstaller : MonoInstaller
    {
        [SerializeField] private Referee _referee;
    
        public override void InstallBindings()
        {
            Container
                .Bind<Referee>()
                .FromInstance(_referee)
                .AsSingle();
        }
    }
}
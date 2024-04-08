using UI.Game;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class HUDInstaller : MonoInstaller
    {
        [SerializeField] private HUD _hud;
    
        public override void InstallBindings()
        {
            Container
                .Bind<HUD>()
                .FromInstance(_hud)
                .AsSingle();
        }
    }
}
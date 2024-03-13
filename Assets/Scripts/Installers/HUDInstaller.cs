using UnityEngine;
using Zenject;

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
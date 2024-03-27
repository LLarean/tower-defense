using UnityEngine;
using Zenject;

public class TowerParamsInstaller : MonoInstaller
{
    [SerializeField] private TowerParams _towerParams;
    
    public override void InstallBindings()
    {
        Container
            .Bind<TowerParams>()
            .FromInstance(_towerParams)
            .AsSingle();
    }
}
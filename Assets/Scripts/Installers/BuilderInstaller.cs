using UnityEngine;
using Zenject;

public class BuilderInstaller : MonoInstaller
{
    [SerializeField] private Builder _builder;
    
    public override void InstallBindings()
    {
        Container
            .Bind<Builder>()
            .FromInstance(_builder)
            .AsSingle();
    }
}
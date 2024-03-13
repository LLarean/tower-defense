using UnityEngine;
using Zenject;

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
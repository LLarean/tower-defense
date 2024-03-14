using UnityEngine;
using Zenject;

public class GameDirectorInstaller : MonoInstaller
{
    [SerializeField] private GameDirector _gameDirector;
    
    public override void InstallBindings()
    {
        Container
            .Bind<GameDirector>()
            .FromInstance(_gameDirector)
            .AsSingle();
    }}
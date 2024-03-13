using UnityEngine;
using Zenject;

public class EnemySpawnInstaller : MonoInstaller
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    
    public override void InstallBindings()
    {
        Container
            .Bind<EnemiesSpawner>()
            .FromInstance(_enemiesSpawner)
            .AsSingle();
    }
}
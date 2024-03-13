using UnityEngine;
using Zenject;

public class EnemySpawnInstaller : MonoInstaller
{
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    
    public override void InstallBindings()
    {
        Container
            .Bind<EnemiesSpawner>()
            .FromInstance(enemiesSpawner)
            .AsSingle();
    }
}
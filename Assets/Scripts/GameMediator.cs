using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GameMediator : MonoBehaviour
{
    [Inject] private EnemiesSpawner _enemiesSpawner;
    [Inject] private Builder _builder;

    [Button()] public void StartMatch() => _enemiesSpawner.StartMatch();
    
    [Button()] public void BuildFireTower() => _builder.BuildFireTower();
    [Button()] public void BuildWaterTower() => _builder.BuildWaterTower();
}
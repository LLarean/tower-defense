using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GameMediator : MonoBehaviour
{
    [Inject] private EnemiesSpawner _enemiesSpawner;
    [Inject] private Builder _builder;

    [Button()] public void StartMatch() => _enemiesSpawner.StartMatch();
    
    [Button()] public void BuildGunTower() => _builder.BuildGunTower();
    [Button()] public void BuildArrowTower() => _builder.BuildArrowTower();
}
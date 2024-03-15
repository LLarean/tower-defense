using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GameMediator : MonoBehaviour
{
    [Inject] private GameDirector _gameDirector;
    [Inject] private Builder _builder;

    [Button()] public void StartMatch() => _gameDirector.StartMatch();
    
    [Button()] public void BuildFireTower() => _builder.BuildFireTower();
    [Button()] public void BuildIceTower() => _builder.BuildWaterTower();
}
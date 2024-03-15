using NaughtyAttributes;
using UI.Game;
using UnityEngine;
using Zenject;

public class GameMediator : MonoBehaviour
{
    [Inject] private GameDirector _gameDirector;
    [Inject] private Builder _builder;
    [Inject] private HUD _hud;

    [Button()] public void StartMatch() => _gameDirector.StartMatch();
    
    [Button()] public void BuildFireTower() => _builder.BuildFireTower();
    [Button()] public void BuildIceTower() => _builder.BuildWaterTower();
    
    [Button()] public void StartClock() => _hud.StartClock();
    [Button()] public void PauseClock() => _hud.PauseClock();
    [Button()] public void ResetClock() => _hud.ResetClock();
    [Button()] public void ClearInfo() => _hud.ClearInfo();
}
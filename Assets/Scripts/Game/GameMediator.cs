using Game;
using NaughtyAttributes;
using UI.Game;
using UnityEngine;
using Zenject;

public class GameMediator : MonoBehaviour
{
    [Inject] private RoundStarter _roundStarter;
    [Inject] private Builder _builder;
    [Inject] private HUD _hud;

    [Button()] public void StartMatch() => _roundStarter.StartMatch();
    [Button()] public void StopMatch() => _roundStarter.StopMatch();
    
    [Button()] public void BuildFireTower() => _hud.BuildFireTower();
    [Button()] public void BuildAirTower() => _hud.BuildAirTower();
    [Button()] public void BuildWaterTower() => _hud.BuildWaterTower();
    [Button()] public void BuildIceTower() => _hud.BuildIceTower();
    
    [Button()] public void StartClock() => _hud.StartClock();
    [Button()] public void PauseClock() => _hud.PauseClock();
    [Button()] public void ResetClock() => _hud.ResetClock();
    [Button()] public void ClearInfo() => _hud.ClearInfo();
}
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class PlayerModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        PlayerModel playerModel = new PlayerModel
        {
            Health = 100,
            Gold = 150,
        };
        
        Container
            .Bind<PlayerModel>()
            .FromInstance(playerModel)
            .AsSingle();
    }
}
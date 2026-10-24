using Zenject;

namespace Installers
{
    public class PlayerModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BuildModel currentBuilding = new BuildModel();
            PlayerModel playerModel = new PlayerModel(100, 150, currentBuilding);

            Container
                .Bind<PlayerModel>()
                .FromInstance(playerModel)
                .AsSingle();
        }
    }
}
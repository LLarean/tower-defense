using ModalWindows;
using Zenject;

namespace Installers
{
    public class ModelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPlayerModel();
            BindConfirmationWindowModel();
        }

        private void BindPlayerModel()
        {
            BuildModel currentBuilding = new BuildModel();
            PlayerModel playerModel = new PlayerModel(100, 150, currentBuilding);

            Container
                .Bind<PlayerModel>()
                .FromInstance(playerModel)
                .AsSingle();
        }
        
        private void BindConfirmationWindowModel()
        {
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel();

            Container
                .Bind<ConfirmationWindowModel>()
                .FromInstance(confirmationWindowModel)
                .AsSingle();
        }
    }
}
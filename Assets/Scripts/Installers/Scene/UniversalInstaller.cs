using Game;
using GameLogic.Navigation;
using GameUtilities;
using ModalWindows;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class UniversalInstaller : MonoInstaller
    {
        [SerializeField] private RoundStarter _roundStarter;
        [SerializeField] private EnemiesRouter _enemiesRouter;
        [FormerlySerializedAs("_enemiesCreator")] [SerializeField] private CreatorEnemies creatorEnemies;
        [SerializeField] private MatchSettings _matchSettings;
        [SerializeField] private NotificationWindow _notificationWindow;
        [SerializeField] private ConfirmationWindow _confirmationWindow;

        public override void InstallBindings()
        {
            BindRoundStarter();
            BindEnemiesRouter();
            BindEnemiesCreator();
            BindMatchSettings();
            BindNotificationWindow();
            BindConfirmationWindow();
        }

        private void BindRoundStarter()
        {
            Container
                .Bind<RoundStarter>()
                .FromInstance(_roundStarter)
                .AsSingle();
        }

        private void BindEnemiesRouter()
        {
            Container
                .Bind<EnemiesRouter>()
                .FromInstance(_enemiesRouter)
                .AsSingle();
        }

        private void BindEnemiesCreator()
        {
            Container
                .Bind<CreatorEnemies>()
                .FromInstance(creatorEnemies)
                .AsSingle();
        }

        private void BindMatchSettings()
        {
            Container
                .Bind<MatchSettings>()
                .FromInstance(_matchSettings)
                .AsSingle();
        }

        private void BindNotificationWindow()
        {
            Container
                .Bind<NotificationWindow>()
                .FromInstance(_notificationWindow)
                .AsSingle();
        }

        private void BindConfirmationWindow()
        {
            Container
                .Bind<ConfirmationWindow>()
                .FromInstance(_confirmationWindow)
                .AsSingle();;
        }
    }
}
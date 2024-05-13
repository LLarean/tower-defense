using Game;
using GameLogic.Navigation;
using GameUtilities;
using ModalWindows;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UniversalInstaller : MonoInstaller
    {
        [SerializeField] private RoundStarter _roundStarter;
        [SerializeField] private EnemiesRouter _enemiesRouter;
        [SerializeField] private CreatorEnemies _creatorEnemies;
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
                .FromInstance(_creatorEnemies)
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
        
        [Button]
        private void SetReferences()
        {
            var roundStarters = FindObjectsOfType<RoundStarter>();

            if (roundStarters != null)
            {
                _roundStarter = roundStarters[0];
            }

            var enemiesRouters = FindObjectsOfType<EnemiesRouter>();

            if (enemiesRouters != null)
            {
                _enemiesRouter = enemiesRouters[0];
            }
            
            var creatorEnemies = FindObjectsOfType<CreatorEnemies>();

            if (creatorEnemies != null)
            {
                _creatorEnemies = creatorEnemies[0];
            }
            
            var matchSettings = FindObjectsOfType<MatchSettings>();
            
            if (matchSettings != null)
            {
                _matchSettings = matchSettings[0];
            }
            
            var notificationWindows = FindObjectsOfType<NotificationWindow>();
            
            if (notificationWindows != null)
            {
                _notificationWindow = notificationWindows[0];
            }
            
            var confirmationWindows = FindObjectsOfType<ConfirmationWindow>();
            
            if (confirmationWindows != null)
            {
                _confirmationWindow = confirmationWindows[0];
            }
        }
    }
}
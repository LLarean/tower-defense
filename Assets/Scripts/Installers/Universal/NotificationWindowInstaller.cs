using ModalWindows;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class NotificationWindowInstaller : MonoInstaller
    {
        [SerializeField] private NotificationWindow _notificationWindow;
    
        public override void InstallBindings()
        {
            Container
                .Bind<NotificationWindow>()
                .FromInstance(_notificationWindow)
                .AsSingle();
        }
    }
}
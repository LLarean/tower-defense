using UnityEngine;
using Zenject;

namespace Installers
{
    public class ModalWindowInstaller : MonoInstaller
    {
        [SerializeField] private ModalWindow _modalWindow;
    
        public override void InstallBindings()
        {
            Container
                .Bind<ModalWindow>()
                .FromInstance(_modalWindow)
                .AsSingle();
        }
    }
}
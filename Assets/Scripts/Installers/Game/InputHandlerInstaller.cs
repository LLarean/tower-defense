using Infrastructure;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class InputHandlerInstaller : MonoInstaller
    {
        [SerializeField] private InputHandler _inputHandler;
    
        public override void InstallBindings()
        {
            Container
                .Bind<InputHandler>()
                .FromInstance(_inputHandler)
                .AsSingle();
        }
    }
}
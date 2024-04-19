using ModalWindows;
using UnityEngine;
using Zenject;

public class ConfirmationWindowInstaller : MonoInstaller
{
    [SerializeField] private ConfirmationWindow _confirmationWindow;
    
    public override void InstallBindings()
    {
        Container
            .Bind<ConfirmationWindow>()
            .FromInstance(_confirmationWindow)
            .AsSingle();
    }
}
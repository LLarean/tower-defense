using Game;
using Menu;
using NaughtyAttributes;
using UI;
using UI.Menu;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuMediator _menuMediator;
        [SerializeField] private MenuReferee _menuReferee;
        [SerializeField] private MainScreen _mainScreen;
        [FormerlySerializedAs("_networkWindow")] [SerializeField] private NetworkWindowTemp networkWindowTemp;
        [SerializeField] private SettingsWindow _settingsWindow;

        public override void InstallBindings()
        {
            BindMenuMediator();
            BindMenuReferee();
            BindMainScreen();
            BindNetworkWindow();
            BindSettingsWindow();
        }

        private void BindMenuMediator()
        {
            Container
                .Bind<MenuMediator>()
                .FromInstance(_menuMediator)
                .AsSingle();
        }

        private void BindMenuReferee()
        {
            Container
                .Bind<MenuReferee>()
                .FromInstance(_menuReferee)
                .AsSingle();
        }

        private void BindMainScreen()
        {
            Container
                .Bind<MainScreen>()
                .FromInstance(_mainScreen)
                .AsSingle();
        }
        
        private void BindNetworkWindow()
        {
            Container
                .Bind<NetworkWindowTemp>()
                .FromInstance(networkWindowTemp)
                .AsSingle();
        }

        private void BindSettingsWindow()
        {
            Container
                .Bind<SettingsWindow>()
                .FromInstance(_settingsWindow)
                .AsSingle();
        }

        [Button]
        private void SetReferences()
        {
            var menuMediators = FindObjectsOfType<MenuMediator>();

            if (menuMediators != null)
            {
                _menuMediator = menuMediators[0];
            }

            var menuReferees = FindObjectsOfType<MenuReferee>();

            if (menuReferees != null)
            {
                _menuReferee = menuReferees[0];
            }
            
            var mainScreens = FindObjectsOfType<MainScreen>();

            if (mainScreens != null)
            {
                _mainScreen = mainScreens[0];
            }
            
            var networkWindows = FindObjectsOfType<NetworkWindowTemp>();

            if (networkWindows != null)
            {
                networkWindowTemp = networkWindows[0];
            }
            
            var settingsWindows = FindObjectsOfType<SettingsWindow>();

            if (settingsWindows != null)
            {
                _settingsWindow = settingsWindows[0];
            }
        }
    }
}
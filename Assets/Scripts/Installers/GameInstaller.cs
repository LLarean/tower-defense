using Game;
using UI.Game;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameMediator _gameMediator;
        [SerializeField] private Referee _referee;
        [SerializeField] private Builder _builder;
        [SerializeField] private HUD _hud;
        
        public override void InstallBindings()
        {
            BindGameMediator();
            BindReferee();
            BindBuilder();
            BindHUD();
        }

        private void BindGameMediator()
        {
            Container
                .Bind<GameMediator>()
                .FromInstance(_gameMediator)
                .AsSingle();;
        }
        
        private void BindReferee()
        {
            Container
                .Bind<Referee>()
                .FromInstance(_referee)
                .AsSingle();;
        }
        
        private void BindBuilder()
        {
            Container
                .Bind<Builder>()
                .FromInstance(_builder)
                .AsSingle();
        }

        private void BindHUD()
        {
            Container
                .Bind<HUD>()
                .FromInstance(_hud)
                .AsSingle();
        }
    }
}
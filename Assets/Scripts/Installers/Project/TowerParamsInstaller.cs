using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class TowerParamsInstaller : MonoInstaller
    {
        [SerializeField] private TowerParams _towerParams;
    
        public override void InstallBindings()
        {
            Container
                .Bind<TowerParams>()
                .FromInstance(_towerParams)
                .AsSingle();
        }
    }
}
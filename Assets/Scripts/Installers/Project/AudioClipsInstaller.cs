using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class AudioClipsInstaller : MonoInstaller
    {
        [SerializeField] private AudioClips _audioClips;
    
        public override void InstallBindings()
        {
            Container
                .Bind<AudioClips>()
                .FromInstance(_audioClips)
                .AsSingle();
        }}
}
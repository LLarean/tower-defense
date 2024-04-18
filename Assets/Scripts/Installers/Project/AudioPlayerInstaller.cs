using UnityEngine;
using Zenject;

public class AudioPlayerInstaller : MonoInstaller
{

    [SerializeField] private AudioPlayer _audioPlayer;
    
    public override void InstallBindings()
    {
        Container
            .Bind<AudioPlayer>()
            .FromComponentInNewPrefab(_audioPlayer)
            .AsSingle();
    }
}
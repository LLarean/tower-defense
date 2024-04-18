using Infrastructure;
using ScriptableObjects;
using UnityEngine;
using Zenject;

public class AudioPlayer : MonoBehaviour, ISoundHandler
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sounds;

    private AudioClips _audioClips;

    [Inject]
    public void Construct(AudioClips audioClips)
    {
        _audioClips = audioClips;
    }
    
    public void HandleLoadMenu()
    {
        _music.clip = _audioClips.MenuMusic;
        _music.Play();
    }

    public void HandleLoadGame()
    {
        _music.clip = _audioClips.BattleMusic;
        _music.Play();
    }

    public void HandleClick() => _sounds.PlayOneShot(_audioClips.ClickSound);
    
    public void HandleCast() => _sounds.PlayOneShot(_audioClips.FireballSound);

    private void Awake() => DontDestroyOnLoad(gameObject);
    
    private void Start() => EventBus.Subscribe(this);

    private void OnDestroy() => EventBus.Unsubscribe(this);
}
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

    public void HandleMusicVolume(float value) => _music.volume = value;

    public void HandleSoundVolume(float value) => _sounds.volume = value;

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
    
    private void Start()
    {
        EventBus.Subscribe(this);
        SetSlidersValue();
    }

    private void OnDestroy() => EventBus.Unsubscribe(this);
    
    private void SetSlidersValue()
    {
        var musicVolume = PlayerPrefs.GetFloat(GlobalStrings.MusicValue, GlobalParams.DefaultMusicVolume);
        _music.volume = musicVolume;

        var soundValue = PlayerPrefs.GetFloat(GlobalStrings.SoundValue, GlobalParams.DefaultSoundVolume);
        _sounds.volume = soundValue;
    }
}
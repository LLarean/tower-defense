using Infrastructure;
using ScriptableObjects;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, ISoundHandler
{
    [Header("Audio sources")]
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sounds;
    [Header("Storage of sounds and music")]
    [SerializeField] private AudioClips _audioClips;

    public void HandleMusicVolume(float value) => _music.volume = value;

    public void HandleSoundVolume(float value) => _sounds.volume = value;

    public void HandleLoadMenuScene() =>  PlayMusic(_audioClips.MenuMusic);

    public void HandleLoadGameScene() => PlayMusic(_audioClips.BattleMusic);

    public void HandleClick() => _sounds.PlayOneShot(_audioClips.ClickSound);
    
    public void HandleConstruction() => _sounds.PlayOneShot(_audioClips.СonstructionSound);

    public void HandleCast() => _sounds.PlayOneShot(_audioClips.FireballSound);

    private void Awake() => DontDestroyOnLoad(gameObject);

    private void Start()
    {
        EventBus.Subscribe(this);
        SetVolume();
    }

    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void SetVolume()
    {
        var musicVolume = PlayerPrefs.GetFloat(GlobalStrings.MusicValue, GlobalParams.DefaultMusicVolume);
        _music.volume = musicVolume;

        var soundValue = PlayerPrefs.GetFloat(GlobalStrings.SoundValue, GlobalParams.DefaultSoundVolume);
        _sounds.volume = soundValue;
    }

    private void PlayMusic(AudioClip audioClip)
    {
        _music.clip = audioClip;
        _music.loop = true;
        _music.Play();
    }
}
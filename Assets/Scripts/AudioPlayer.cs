using Infrastructure;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, ISoundHandler
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sounds;
    [Header("Music")]
    [SerializeField] private AudioClip _menu;
    [SerializeField] private AudioClip _battle;
    [Header("Sounds")]
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _fireball;

    public void HandleClick() => _sounds.PlayOneShot(_click);
    
    public void HandleCast() => _sounds.PlayOneShot(_fireball);

    public void HandleLoadMenu()
    {
        _music.clip = _menu;
        _music.Play();
    }

    public void HandleLoadGame()
    {
        _music.clip = _battle;
        _music.Play();
    }
    
    private void Awake() => DontDestroyOnLoad(gameObject);
    
    private void Start() => EventBus.Subscribe(this);

    private void OnDestroy() => EventBus.Unsubscribe(this);
}
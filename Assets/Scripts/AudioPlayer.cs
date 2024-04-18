using Infrastructure;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, ISoundHandler
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sounds;
    [Space]
    [SerializeField] private AudioClip _menu;
    [SerializeField] private AudioClip _battle;
    [SerializeField] private AudioClip _click;

    public void HandleClick()
    {
        _sounds.PlayOneShot(_click);
    }

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
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        EventBus.Subscribe(this);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(this);
    }
}
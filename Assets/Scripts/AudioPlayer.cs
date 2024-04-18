using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sounds;
    [Space]
    [SerializeField] private AudioClip _menu;
    [SerializeField] private AudioClip _battle;

    public void PlayMenuMusic()
    {
        _music.clip = _menu;
        _music.Play();
    }
    
    public void PlayBattleMusic()
    {
        _music.clip = _battle;
        _music.Play();
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioClips", menuName = "ScriptableObjects/AudioClips", order = 1)]
    public class AudioClips : ScriptableObject
    {
        [Header("Music")]
        [SerializeField] private AudioClip _menu;
        [SerializeField] private AudioClip _battle;
        [Header("Sounds")]
        [SerializeField] private AudioClip _click;
        [SerializeField] private AudioClip _fireball;

        public AudioClip MenuMusic => _menu;
        public AudioClip BattleMusic => _battle;
        public AudioClip ClickSound => _click;
        public AudioClip FireballSound => _fireball;
    }
}

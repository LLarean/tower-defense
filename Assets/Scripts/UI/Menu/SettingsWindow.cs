using Infrastructure;
using ModalWindows;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsWindow : ModalWindow
    {
        [Space]
        [SerializeField] private Button _close;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sounds;

        private void Start()
        {
            _close.onClick.AddListener(CloseWindow);
            _music.onValueChanged.AddListener(ChangeMusicVolume);
            _sounds.onValueChanged.AddListener(ChangeSoundVolume);

            SetSlidersValue();
        }

        private void CloseWindow()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            Hide();
        }

        private void ChangeMusicVolume(float value)
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleMusicVolume(value));
        }

        private void ChangeSoundVolume(float value)
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleSoundVolume(value));
        }

        private void SetSlidersValue()
        {
            var musicVolume = PlayerPrefs.GetFloat(GlobalStrings.MusicValue, GlobalParams.DefaultMusicVolume);
            _music.value = musicVolume;

            var soundValue = PlayerPrefs.GetFloat(GlobalStrings.SoundValue, GlobalParams.DefaultSoundVolume);
            _sounds.value = soundValue;
        }
    }
}
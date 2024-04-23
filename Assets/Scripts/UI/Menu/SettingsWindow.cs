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
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _soundsVolume;

        private void Start()
        {
            _close.onClick.AddListener(CloseWindow);
            _musicVolume.onValueChanged.AddListener(ChangeMusicVolume);
            _soundsVolume.onValueChanged.AddListener(ChangeSoundVolume);

            SetSlidersValue();
        }

        private void CloseWindow()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            Hide();
        }

        private void ChangeMusicVolume(float value)
        {
            PlayerPrefs.SetFloat(GlobalStrings.MusicValue, value);
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleMusicVolume(value));
        }

        private void ChangeSoundVolume(float value)
        {
            PlayerPrefs.SetFloat(GlobalStrings.SoundValue, value);
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleSoundVolume(value));
        }

        private void SetSlidersValue()
        {
            var musicVolume = PlayerPrefs.GetFloat(GlobalStrings.MusicValue, GlobalParams.DefaultMusicVolume);
            _musicVolume.value = musicVolume;

            var soundValue = PlayerPrefs.GetFloat(GlobalStrings.SoundValue, GlobalParams.DefaultSoundVolume);
            _soundsVolume.value = soundValue;
        }
    }
}
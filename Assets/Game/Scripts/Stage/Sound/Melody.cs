using Game.Stage.Hud;
using UnityEngine;

namespace Game.Stage.Sound
{
    public sealed class Melody : IWidget
    {
        private readonly AudioSource _source;
        private readonly Setting _setting;

        public Melody(Transform root, AudioClip clip, float volume, Setting setting)
        {
            _setting = setting;
            var holder = new GameObject("Melody");
            holder.transform.SetParent(root, false);
            _source = holder.AddComponent<AudioSource>();
            _source.clip = clip;
            _source.volume = volume;
            _source.loop = true;
            _source.spatialBlend = 0f;
            _source.ignoreListenerPause = true;
            _source.mute = !setting.On;
            _source.Play();
        }

        public void Refresh()
        {
            if (_source.mute == _setting.On)
            {
                _source.mute = !_setting.On;
            }
        }
    }
}

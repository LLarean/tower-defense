using System.Collections.Generic;
using Game.Stage.Config;
using Game.Stage.Hud;
using UnityEngine;

namespace Game.Stage.Sound
{
    public sealed class Voices
    {
        private const float Wobble = 0.06f;

        private readonly AudioSource[] _sources;
        private readonly Setting _setting;
        private readonly Dictionary<AudioClip, float> _heard = new Dictionary<AudioClip, float>();
        private int _next;

        public Voices(Transform root, int count, Setting setting)
        {
            _setting = setting;
            var holder = new GameObject("Voices");
            holder.transform.SetParent(root, false);
            _sources = new AudioSource[count];
            for (var index = 0; index < count; index++)
            {
                var source = holder.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                source.ignoreListenerPause = true;
                _sources[index] = source;
            }
        }

        public void Play(CueData cue)
        {
            if (cue.Silent || !_setting.On || _sources.Length == 0)
            {
                return;
            }
            var now = Time.unscaledTime;
            if (_heard.TryGetValue(cue.Clip, out var heard) && now - heard < cue.Gap)
            {
                return;
            }
            _heard[cue.Clip] = now;
            var source = _sources[_next];
            _next = (_next + 1) % _sources.Length;
            source.clip = cue.Clip;
            source.volume = cue.Volume;
            source.pitch = 1f + Random.Range(-Wobble, Wobble);
            source.Play();
        }
    }
}

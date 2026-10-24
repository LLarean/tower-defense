using System;
using Game.Core.Battles;

namespace Game.Stage.Net
{
    public sealed class Clock
    {
        private const int MaxSteps = 5;

        private float _pending;

        public void Run(float seconds, Timeline timeline)
        {
            _pending = Math.Min(_pending + seconds, Timeline.Step * MaxSteps);
            while (_pending >= Timeline.Step && timeline.TryStep())
            {
                _pending -= Timeline.Step;
            }
        }
    }
}

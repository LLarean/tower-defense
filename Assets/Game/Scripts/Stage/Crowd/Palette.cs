using Game.Core.Monsters;
using Game.Stage.Config;
using UnityEngine;

namespace Game.Stage.Crowd
{
    public sealed class Palette
    {
        private readonly SpectacleAsset _spectacle;

        public Palette(SpectacleAsset spectacle)
        {
            _spectacle = spectacle;
        }

        public Color Of(Monster monster)
        {
            for (var mark = 0; mark < _spectacle.Marks; mark++)
            {
                if (monster.Has(_spectacle.Condition(mark)))
                {
                    return _spectacle.Color(mark);
                }
            }
            return Color.black;
        }
    }
}

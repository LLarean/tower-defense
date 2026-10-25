using UnityEngine;

namespace Game.Stage.Hud
{
    public sealed class Bare : ICover
    {
        public bool Covers(Vector2 pointer) => false;
    }
}

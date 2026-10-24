using UnityEngine;

namespace Game.Stage.Hud
{
    public interface ICover
    {
        bool Covers(Vector2 pointer);
    }
}

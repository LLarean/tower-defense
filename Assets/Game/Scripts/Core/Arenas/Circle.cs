using System.Numerics;

namespace Game.Core.Arenas
{
    public readonly struct Circle
    {
        public Circle(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Vector3 Center { get; }

        public float Radius { get; }
    }
}

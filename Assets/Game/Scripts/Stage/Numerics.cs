namespace Game.Stage
{
    public static class Numerics
    {
        public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 vector) =>
            new UnityEngine.Vector3(vector.X, vector.Y, vector.Z);

        public static System.Numerics.Vector3 ToCore(this UnityEngine.Vector3 vector) =>
            new System.Numerics.Vector3(vector.x, vector.y, vector.z);
    }
}

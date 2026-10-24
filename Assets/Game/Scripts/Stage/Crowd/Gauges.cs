using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Stage.Crowd
{
    public sealed class Gauges
    {
        private const int Batch = 1023;

        private static readonly int Fill = Shader.PropertyToID("_Fill");
        private static readonly int Tint = Shader.PropertyToID("_Tint");

        private readonly Mesh _mesh;
        private readonly Material _material;
        private readonly Matrix4x4[] _matrices = new Matrix4x4[Batch];
        private readonly float[] _fills = new float[Batch];
        private readonly Vector4[] _tints = new Vector4[Batch];
        private readonly MaterialPropertyBlock _block = new MaterialPropertyBlock();
        private readonly RenderParams _params;
        private Quaternion _facing = Quaternion.identity;
        private int _count;

        public Gauges(Mesh mesh, Material material)
        {
            _mesh = mesh;
            _material = material;
            _params = new RenderParams(material)
            {
                matProps = _block,
                shadowCastingMode = ShadowCastingMode.Off,
                receiveShadows = false,
            };
        }

        public void Face(Camera camera) => _facing = camera.transform.rotation;

        public void Add(Vector3 position, Vector2 size, float fill, Color tint)
        {
            if (_count == Batch)
            {
                Draw();
            }
            _matrices[_count] = Matrix4x4.TRS(position, _facing, new Vector3(size.x, size.y, 1f));
            _fills[_count] = fill;
            _tints[_count] = tint;
            _count++;
        }

        public void Draw()
        {
            if (_count == 0)
            {
                return;
            }
            _block.SetFloatArray(Fill, _fills);
            _block.SetVectorArray(Tint, _tints);
            Graphics.RenderMeshInstanced(_params, _mesh, 0, _matrices, _count);
            _count = 0;
        }
    }
}

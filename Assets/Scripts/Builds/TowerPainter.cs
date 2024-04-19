using UnityEngine;

namespace Builds
{
    public class TowerPainter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _walls;
        [SerializeField] private MeshRenderer _blocks;

        public void SetWhiteColor()
        {
            _walls.material.color = Color.white;
            _blocks.material.color = Color.white;
        }

        public void SetRedColor()
        {
            _walls.material.color = Color.red;
            _blocks.material.color = Color.red;
        }
    }
}
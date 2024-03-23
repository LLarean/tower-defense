using UnityEngine;

namespace Builds
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private MouseFollower _mouseFollower;

        public virtual void Initialize(Collider terrainCollider)
        {
            _mouseFollower.Initialize(terrainCollider);
            _mouseFollower.enabled = true;
        }

        public void DisableMouseFollower()
        {
            _mouseFollower.enabled = false;
        }

        public void MousePositionChange(float positionX, float positionY)
        {
            if (_mouseFollower.isActiveAndEnabled == true)
            {
                _mouseFollower.MousePositionChange(positionX, positionY);
            }
        }
    }
}

using UnityEngine;

namespace Builds
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private MouseFollower _mouseFollower;

        public virtual void Initialize(Builder builder, Collider terrainCollider)
        {
            _mouseFollower.Init(builder, terrainCollider);
            _mouseFollower.enabled = true;
        }

        public void DisableMouseFollower()
        {
            _mouseFollower.enabled = false;
        }

        public void MousePositionChange(float positionX, float positionY)
        {
            _mouseFollower.MousePositionChange(positionX, positionY);
        }
    }
}

using UnityEngine;

namespace Builds
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private MouseFollower _mouseFollower;

        public virtual void Initialize(Builder builder, Collider terrainCollider)
        {
            _mouseFollower.Init(builder, terrainCollider);

            _mouseFollower.enabled = true;
            // EnableBuildingFollower();
        }
    }
}

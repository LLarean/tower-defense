using GameUtilities;
using UnityEngine;

namespace Builds
{
    public class Building : MonoBehaviour
    {
        [SerializeField] protected bool IsBuilt;
        
        private MouseFollower _mouseFollower;
        private bool _canBuild = true;
        
        public bool CanBuild => _canBuild;
        
        public virtual void Initialize(Collider terrainCollider)
        {
            _mouseFollower = new MouseFollower(terrainCollider);
        }

        public void MousePositionChange(Vector2 mousePosition)
        {
            if (_mouseFollower == null)
            {
                Debug.LogError("Class: 'Building', Method: 'MousePositionChange', Message: '_mouseFollower == null'");
                return;
            }
            
            _canBuild = _mouseFollower.TryGetBuildPosition(mousePosition, out Vector3 buildPosition);

            if (_canBuild == true)
            {
                transform.position = buildPosition;
            }
        }

        public void DisableConstructionMode()
        {
            IsBuilt = true;
            _canBuild = false;
        }
    }
}
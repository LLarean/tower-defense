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
        
        

        public void DisableConstructionMode()
        {
            IsBuilt = true;
            _canBuild = false;
        }
    }
}
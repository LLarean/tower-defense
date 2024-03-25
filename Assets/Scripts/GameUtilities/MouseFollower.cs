using System;
using UnityEngine;

namespace GameUtilities
{
    public class MouseFollower
    {
        private Collider _terrainCollider;
        private Camera _mainCamera;

        public MouseFollower(Collider terrainCollider)
        {
            Type type = typeof(MouseFollower);
            string className = type.Name;
        
            if (terrainCollider != null)
            {
                _terrainCollider = terrainCollider;
            }
            else
            {
                Debug.LogError($"Class: '{className}', Method: 'ctor', Message: 'terrainCollider == null'");
            }
        
            if (Camera.main != null)
            {
                _mainCamera = Camera.main;
            }
            else
            {
                Debug.LogError($"Class: '{className}', Method: 'ctor', Message: 'Camera.main == null'");
            }
        
        }
    
        public bool TryGetBuildPosition(Vector2 mousePosition, out Vector3 buildPosition)
        {
            bool isSuccess = false;
            buildPosition = Vector3.zero;
            
            mousePosition.x = Mathf.Round(mousePosition.x / GlobalParams.MovingStep) * GlobalParams.MovingStep;
            mousePosition.y = Mathf.Round(mousePosition.y / GlobalParams.MovingStep) * GlobalParams.MovingStep;

            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0));
            RaycastHit hit;

            if (_terrainCollider.Raycast(ray, out hit, Mathf.Infinity) == true)
            {
                buildPosition = hit.point + new Vector3(0, GlobalParams.HeightOffset, 0);
                isSuccess = true;
            }

            return isSuccess;
        }
    }
}
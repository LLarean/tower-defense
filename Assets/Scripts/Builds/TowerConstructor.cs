using Units;
using UnityEngine;

namespace Builds
{
    public class TowerConstructor
    {
        private Collider _constructionArea;
        private Collider _castArea;
        
        private bool _isBuilt;
        private bool _canBuilt;

        public TowerConstructor(Collider constructionArea, Collider castArea)
        {
            _constructionArea = constructionArea;
            _castArea = castArea;
        }
        
        public bool IsBuilt => _isBuilt;
        public bool CanBuilt => _canBuilt;

        public void TriggerEnter(Collider collider)
        {
            if (_isBuilt == false)
            {
                var isSuccess = collider.TryGetComponent(out Tower tower);

                if (isSuccess == true)
                {
                    _canBuilt = false;
                }
            }
            else
            {
                // var isAvailable = collision.TryGetComponent(out Enemy enemy);

                // if (isAvailable == false)
                // {
                    // return;
                // }
            }
        }
    }
}
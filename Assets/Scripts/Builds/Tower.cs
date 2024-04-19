using Units;
using UnityEngine;

namespace Builds
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private SpellCaster _spellCaster;
        [SerializeField] private BuildArea _buildArea;
        [SerializeField] private CastArea _castArea;
        [SerializeField] private TowerPainter _towerPainter;
        [Space]
        [SerializeField] private bool _isBuilt;
        
        private TowerConstructor _towerConstructor;
        private bool _canBuilt = true;
        
        public bool IsBuilt => _isBuilt;
        public bool CanBuilt => _canBuilt;

        public void Initialize(TowerModel towerModel, CastItem castItem)
        {
            if (towerModel == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'Initialize', Message: 'towerModel == null'");
                return;
            }
            
            if (castItem == null)
            {
                Debug.LogError("Class: 'Tower', Method: 'Initialize', Message: 'castItem == null'");
                return;
            }
            
            _spellCaster.Initialize(towerModel, castItem);

            _buildArea.TriggerEnter += BuildTriggerEnter;
            _buildArea.TriggerExit += BuildTriggerExit;
            
            _castArea.TriggerEnter += CastTriggerEnter;
            _castArea.TriggerExit += CastTriggerExit;
        }
        
        public void DisableConstructionMode() => _isBuilt = true;

        private void BuildTriggerEnter(Collider collider)
        {
            if (_isBuilt == true)
            {
                return;
            }
            
            _canBuilt = false;
            _towerPainter.SetRedColor();
        }

        private void BuildTriggerExit(Collider collider)
        {
            if (_isBuilt == true)
            {
                return;
            }
            
            _canBuilt = true;
            _towerPainter.SetWhiteColor();
        }

        private void CastTriggerEnter(Collider collider)
        {
            if (_isBuilt == false)
            {
                return;
            }
            
            var isAvailable = collider.TryGetComponent(out Enemy enemy);
            
            if (isAvailable == false)
            {
                return;
            }
            
            _spellCaster.SetTarget(enemy.gameObject.transform);
        }

        private void CastTriggerExit(Collider collider)
        {
            if (_isBuilt == false)
            {
                return;
            }
            
            _spellCaster.ResetTarget();
        }
    }
}
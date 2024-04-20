using Units;
using UnityEngine;
using Utilities;

namespace Builds
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private SpellCaster _spellCaster;
        [SerializeField] private BuildArea _buildArea;
        [SerializeField] private CastArea _castArea;
        [SerializeField] private TowerPainter _towerPainter;
        [Space]
        [SerializeField] private ElementalType _elementalType;
        [SerializeField] private CastItem _castItem;
        [SerializeField] private bool _isBuilt;
        
        private TowerConstructor _towerConstructor;
        private bool _canBuilt = true;
        
        public ElementalType ElementalType => _elementalType;
        // public bool IsBuilt => _isBuilt;
        public bool CanBuilt => _canBuilt;

        public void Initialize(TowerModel towerModel)
        {
            if (towerModel == null)
            {
                CustomLogger.LogError("towerModel == null");
                return;
            }
            
            if (_castItem == null)
            {
                CustomLogger.LogError("_castItem == null");
                return;
            }

            CastItemModel castItemModel = new CastItemModel
            {
                ElementalType = towerModel.ElementalType,
                Damage = towerModel.Damage,
            };
            
            _castItem.Initialize(castItemModel);
            _spellCaster.Initialize(_castItem, towerModel.AttackSpeed);

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
            
            // TODO Refactor this code
            var isEnemy = collider.TryGetComponent(out Enemy enemy);
            var isTower = collider.TryGetComponent(out BuildArea buildArea);
            
            if (isEnemy == true || isTower == true)
            {
                _canBuilt = false;
                _towerPainter.SetRedColor();
            }
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
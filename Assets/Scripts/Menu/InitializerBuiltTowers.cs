using System.Collections.Generic;
using Builds;
using ScriptableObjects;
using UnityEngine;
using Utilities;
using Zenject;

namespace Menu
{
    public class InitializerBuiltTowers : MonoBehaviour
    {
        [SerializeField] private List<Tower> _builtTowers = new();

        [Inject]
        public void Construction(TowerParams towerParams)
        {
            InitializeTowers(towerParams);
        }
        
        private void InitializeTowers(TowerParams towerParams)
        {
            if (towerParams == null)
            {
                CustomLogger.LogError("towerParams == null");
                return;
            }
            
            if (_builtTowers.Count == 0)
            {
                CustomLogger.LogWarning("_towers.Count == 0");
            }
            
            foreach (var tower in _builtTowers)
            {
                towerParams.TryGetTowerContainer(tower.ElementalType, out TowerContainer towerContainer);
                tower.Initialize(towerContainer.TowerModel);
            }
        }
    }
}

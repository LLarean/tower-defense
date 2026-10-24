using System.Collections.Generic;
using Towers;
using ScriptableObjects;
using UnityEngine;
using Utilities;
using Zenject;

namespace GameUtilities
{
    public class MenuRoundStarter : RoundStarter
    {
        [SerializeField] private List<Tower> _towers = new List<Tower>();

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
            
            if (_towers.Count == 0)
            {
                CustomLogger.LogWarning("_towers.Count == 0");
            }
            
            foreach (var tower in _towers)
            {
                towerParams.TryGetTowerContainer(tower.ElementalType, out TowerContainer towerContainer);
                tower.Initialize(towerContainer.TowerModel);
            }
        }
    }
}
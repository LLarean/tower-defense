using System.Collections.Generic;
using Builds;
using ScriptableObjects;
using UnityEngine;
using Utilities;
using Zenject;

namespace Menu
{
    public class MenuBuilder : MonoBehaviour
    {
        [SerializeField] private List<Tower> _towers = new List<Tower>();

        [Inject]
        public void Construct(TowerParams towerParams)
        {
            // TODO Make tower initialization more flexible
            if (towerParams == null)
            {
                CustomLogger.LogMessage("towerParams == null", 2);
                return;
            }

            if (_towers.Count != 3)
            {
                CustomLogger.LogMessage("_towers.Count != 3", 2);
                return;
            }
            
            towerParams.TryGetTowerContainer(ElementalType.Poison, out TowerContainer towerContainer);
            _towers[0].Initialize(towerContainer.TowerModel, towerContainer.CastItem);
         
            towerParams.TryGetTowerContainer(ElementalType.Ice, out towerContainer);
            _towers[1].Initialize(towerContainer.TowerModel, towerContainer.CastItem);
            
            towerParams.TryGetTowerContainer(ElementalType.Fire, out towerContainer);
            _towers[2].Initialize(towerContainer.TowerModel, towerContainer.CastItem);
        }
    }
}

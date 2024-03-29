using System.Collections.Generic;
using Builds;
using UnityEngine;
using Zenject;

namespace Menu
{
    public class MenuBuilder : MonoBehaviour
    {
        [SerializeField] private List<Tower> _towers = new List<Tower>();

        [Inject]
        public void Construct(TowerParams towerParams)
        {
            towerParams.TryGetTowerContainer(ElementalType.Fire, out TowerContainer towerContainer);
            _towers[0].Initialize(towerContainer.TowerModel, towerContainer.CastItem);
            towerParams.TryGetTowerContainer(ElementalType.Ice, out towerContainer);
            _towers[1].Initialize(towerContainer.TowerModel, towerContainer.CastItem);
            towerParams.TryGetTowerContainer(ElementalType.Poison, out towerContainer);
            _towers[2].Initialize(towerContainer.TowerModel, towerContainer.CastItem);
        }
    }
}

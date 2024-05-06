using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TowerParams", menuName = "ScriptableObjects/TowerParams", order = 1)]
    public class TowerParams : ScriptableObject
    {
        [SerializeField] private List<TowerContainer> _towerContainers;

        public bool TryGetTowerContainer(ElementalType elementalType, out TowerContainer towerContainer)
        {
            bool isSuccess = false;
            towerContainer = null;

            foreach (var model in _towerContainers)
            {
                if (model.TowerModel.ElementalType == elementalType)
                {
                    towerContainer = model;
                    isSuccess = true;
                    return isSuccess;
                }
            }
        
            return isSuccess;
        }
    }
}
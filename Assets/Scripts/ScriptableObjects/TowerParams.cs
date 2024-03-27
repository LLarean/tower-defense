using System.Collections.Generic;
using Builds;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerParams", order = 1)]
public class TowerParams : ScriptableObject
{
    [SerializeField] private List<TowerModel> _towerModels;

    public bool TryGetTowerModel(ElementalType elementalType, out TowerModel towerModel)
    {
        bool isSuccess = false;
        towerModel = null;

        foreach (var model in _towerModels)
        {
            if (model.ElementalType == elementalType)
            {
                towerModel = model;
                isSuccess = true;
                return isSuccess;
            }
        }
        
        return isSuccess;
    }
}
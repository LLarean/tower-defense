using System.Collections.Generic;
using Builds;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerParams", order = 1)]
public class TowerParams : ScriptableObject
{
    public List<TowerModel> TowerModels;
    
    // [Range(0, 20)] public int FireTowerDamage;
    // public int FireTowerPrice;
    // [Space]
    // public int IceTowerDamage;
    // public int IceTowerPrice;
}
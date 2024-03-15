using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerParams", order = 1)]
public class TowerParams : ScriptableObject
{
    public int FireTowerDamage;
    public int FireTowerPrice;
    [Space]
    public int IceTowerDamage;
    public int IceTowerPrice;
}
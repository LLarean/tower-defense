using System.Collections.Generic;
using Builds;
using UnityEngine;

public class ConstructedBuildings : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings;

    public void SetNewBuilding(Building building)
    {
        _buildings.Add(building);
    }

}

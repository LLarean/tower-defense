using System.Collections.Generic;
using Builds;

namespace Game
{
    public class ConstructedBuildings
    {
        private List<Building> _buildings = new List<Building>();

        public void SetNewBuilding(Building building)
        {
            _buildings.Add(building);
        }

    }
}
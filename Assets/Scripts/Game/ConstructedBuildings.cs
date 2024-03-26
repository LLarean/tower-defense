using System.Collections.Generic;
using Builds;

namespace Game
{
    public class ConstructedBuildings
    {
        private List<Building> _buildings;

        public void SetNewBuilding(Building building)
        {
            _buildings.Add(building);
        }

    }
}

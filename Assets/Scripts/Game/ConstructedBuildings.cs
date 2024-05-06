using System.Collections.Generic;
using Towers;

namespace Game
{
    public class ConstructedBuildings
    {
        private List<Tower> _towers = new List<Tower>();

        public void SetNewBuilding(Tower tower)
        {
            _towers.Add(tower);
        }

    }
}
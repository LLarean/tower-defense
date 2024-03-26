using Builds;

namespace UI.Game
{
    public static class KeeperBuildingModels
    {
        public static BuildModel GetFireTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.FireTower,
                CastType = CastType.Fire,
                Damage = GlobalParams.FireTowerDamage,
                Price = GlobalParams.FireTowerPrice,
            };

            return buildModel;
        }

        public static BuildModel GetAirTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.AirTower,
                CastType = CastType.Air,
                Damage = GlobalParams.AirTowerDamage,
                Price = GlobalParams.AirTowerPrice,
            };

            return buildModel;
        }

        public static BuildModel GetWaterTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.WaterTower,
                CastType = CastType.Water,
                Damage = GlobalParams.WaterTowerDamage,
                Price = GlobalParams.WaterTowerPrice,
            };

            return buildModel;
        }
    
        public static BuildModel GetIceTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.IceTower,
                CastType = CastType.Ice,
                Damage = GlobalParams.IceTowerDamage,
                Price = GlobalParams.IceTowerPrice,
            };

            return buildModel;
        }
    }
}
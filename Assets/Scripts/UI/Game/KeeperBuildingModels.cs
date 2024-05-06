using Towers;

namespace UI.Game
{
    public static class KeeperBuildingModels
    {
        public static BuildModel GetFireTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.FireTower,
                ElementalType = ElementalType.Fire,
                Damage = GlobalParams.FireTowerDamage,
                Price = GlobalParams.FireTowerPrice,
            };

            return buildModel;
        }

        public static BuildModel GetPoisonTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.PoisonTower,
                ElementalType = ElementalType.Poison,
                Damage = GlobalParams.PoisonTowerDamage,
                Price = GlobalParams.PoisonTowerPrice,
            };

            return buildModel;
        }

        public static BuildModel GetWaterTowerModel()
        {
            BuildModel buildModel = new BuildModel
            {
                Title = GlobalStrings.WaterTower,
                ElementalType = ElementalType.Water,
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
                ElementalType = ElementalType.Ice,
                Damage = GlobalParams.IceTowerDamage,
                Price = GlobalParams.IceTowerPrice,
            };

            return buildModel;
        }
    }
}
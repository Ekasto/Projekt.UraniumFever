using System.Collections.Generic;

namespace UraniumFever.Game
{
    /// <summary>
    /// Database of all building definitions and their properties.
    /// </summary>
    public static class BuildingLibrary
    {
        private static readonly Dictionary<BuildingType, BuildingCost> _costs = new Dictionary<BuildingType, BuildingCost>
        {
            { BuildingType.Bridge, new BuildingCost(primaryCount: 2, secondaryCount: 1) },
            { BuildingType.Defense, new BuildingCost(primaryCount: 7, secondaryCount: 1) },
            { BuildingType.Factory, new BuildingCost(primaryCount: 2, secondaryCount: 1) },
            { BuildingType.Research, new BuildingCost(primaryCount: 5, secondaryCount: 1) },
            { BuildingType.CheapHouse, new BuildingCost(primaryCount: 2, secondaryCount: 1) },
            { BuildingType.Car, BuildingCost.Free() },
            { BuildingType.BrokenCar, new BuildingCost(primaryCount: 3, secondaryCount: 1) },
            { BuildingType.HouseUpgrade, new BuildingCost(primaryCount: 3, secondaryCount: 1) }
        };

        private static readonly Dictionary<BuildingType, string> _displayNames = new Dictionary<BuildingType, string>
        {
            { BuildingType.Bridge, "Bridge" },
            { BuildingType.Defense, "Defense" },
            { BuildingType.Factory, "Factory" },
            { BuildingType.Research, "Research" },
            { BuildingType.CheapHouse, "House" },
            { BuildingType.Car, "Car" },
            { BuildingType.BrokenCar, "Broken Car" },
            { BuildingType.HouseUpgrade, "House Upgrade" }
        };

        public static BuildingCost GetCost(BuildingType buildingType)
        {
            return _costs[buildingType];
        }

        public static string GetDisplayName(BuildingType buildingType)
        {
            return _displayNames[buildingType];
        }

        public static bool IsHouse(BuildingType buildingType)
        {
            return buildingType == BuildingType.CheapHouse;
        }
    }
}

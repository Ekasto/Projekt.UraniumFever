using System.Collections.Generic;
using UnityEngine;

namespace UraniumFever.Game
{
    /// <summary>
    /// Represents a player with their headquarters and resource inventory.
    /// </summary>
    public class Player
    {
        public int PlayerId { get; private set; }
        public ResourceType HQType { get; private set; }
        public Vector2Int HQPosition { get; private set; }

        private Dictionary<ResourceType, int> _inventory;
        private HashSet<Vector2Int> _upgradedBuildings; // Buildings with HouseUpgrade (immune to earthquakes/tornadoes)

        public Player(int playerId, ResourceType hqType, Vector2Int hqPosition)
        {
            PlayerId = playerId;
            HQType = hqType;
            HQPosition = hqPosition;
            _inventory = new Dictionary<ResourceType, int>
            {
                { ResourceType.Electricity, 0 },
                { ResourceType.Food, 0 },
                { ResourceType.Medicine, 0 },
                { ResourceType.PlayerChoice, 0 }
            };
            _upgradedBuildings = new HashSet<Vector2Int>();
        }

        public void AddResource(ResourceType type)
        {
            _inventory[type]++;
        }

        public bool RemoveResource(ResourceType type, int amount = 1)
        {
            if (_inventory[type] < amount)
                return false;

            _inventory[type] -= amount;
            return true;
        }

        public int GetResourceCount(ResourceType type)
        {
            return _inventory[type];
        }

        public int GetTotalResourceCount()
        {
            int total = 0;
            foreach (var count in _inventory.Values)
            {
                total += count;
            }
            return total;
        }

        /// <summary>
        /// Upgrades a building at the specified position, making it immune to earthquakes and tornadoes.
        /// </summary>
        public void UpgradeBuilding(Vector2Int position)
        {
            _upgradedBuildings.Add(position);
        }

        /// <summary>
        /// Checks if a building at the specified position has been upgraded.
        /// </summary>
        public bool IsBuildingUpgraded(Vector2Int position)
        {
            return _upgradedBuildings.Contains(position);
        }
    }
}

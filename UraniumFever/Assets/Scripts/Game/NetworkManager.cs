using UnityEngine;
using UraniumFever.Core;
using System.Collections.Generic;

namespace UraniumFever.Game
{
    /// <summary>
    /// Manages building networks and calculates effects from connected buildings.
    /// Players share bonuses when their networks are connected via bridges.
    /// </summary>
    public class NetworkManager
    {
        private GridManager _gridManager;
        private Player[] _players;

        public NetworkManager(GridManager gridManager, Player[] players)
        {
            _gridManager = gridManager;
            _players = players;
        }

        /// <summary>
        /// Gets all buildings in a player's connected network.
        /// Includes buildings from other players if networks are connected.
        /// </summary>
        public List<GridTile> GetConnectedBuildings(Player player)
        {
            return BridgeConnectivity.GetBuildingsInNetwork(player.HQPosition, _gridManager);
        }

        /// <summary>
        /// Calculates the resource bonus for a player when drawing cards.
        /// Factory: +4 resources, House: +1 resource.
        /// </summary>
        public int GetResourceBonus(Player player, ResourceType resourceType)
        {
            List<GridTile> buildings = GetConnectedBuildings(player);
            int bonus = 0;

            foreach (GridTile tile in buildings)
            {
                if (tile.Building == BuildingType.Factory)
                {
                    bonus += 4; // Factory gives +4
                }
                else if (tile.Building == BuildingType.CheapHouse)
                {
                    bonus += 1; // House gives +1
                }
            }

            return bonus;
        }

        /// <summary>
        /// Checks if player has Research building in network.
        /// Research allows drawing twice per turn.
        /// </summary>
        public bool HasResearchBonus(Player player)
        {
            List<GridTile> buildings = GetConnectedBuildings(player);

            foreach (GridTile tile in buildings)
            {
                if (tile.Building == BuildingType.Research)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the number of Defense buildings in player's network.
        /// Each Defense reduces disaster strength by 1.
        /// </summary>
        public int GetDisasterReduction(Player player)
        {
            List<GridTile> buildings = GetConnectedBuildings(player);
            int defenseCount = 0;

            foreach (GridTile tile in buildings)
            {
                if (tile.Building == BuildingType.Defense)
                {
                    defenseCount++;
                }
            }

            return defenseCount;
        }

        /// <summary>
        /// Gets all players whose networks are connected to the specified player.
        /// Returns list including the player themselves.
        /// </summary>
        public List<Player> GetConnectedPlayers(Player player)
        {
            List<Player> connectedPlayers = new List<Player>();
            connectedPlayers.Add(player); // Always include self

            foreach (Player otherPlayer in _players)
            {
                if (otherPlayer.PlayerId == player.PlayerId)
                    continue;

                // Check if networks are connected
                if (BridgeConnectivity.AreNetworksConnected(player.HQPosition, otherPlayer.HQPosition, _gridManager))
                {
                    connectedPlayers.Add(otherPlayer);
                }
            }

            return connectedPlayers;
        }

        /// <summary>
        /// Gets the total card draw count for a player.
        /// Base 1 + Houses in network.
        /// </summary>
        public int GetCardDrawCount(Player player)
        {
            List<GridTile> buildings = GetConnectedBuildings(player);
            int houseCount = 0;

            foreach (GridTile tile in buildings)
            {
                if (tile.Building == BuildingType.CheapHouse)
                {
                    houseCount++;
                }
            }

            return 1 + houseCount; // Base 1 + houses
        }

        /// <summary>
        /// Gets a formatted string describing the player's network bonuses.
        /// </summary>
        public string GetBonusDescription(Player player)
        {
            List<GridTile> buildings = GetConnectedBuildings(player);
            int factories = 0;
            int houses = 0;
            int defense = 0;
            bool hasResearch = false;

            foreach (GridTile tile in buildings)
            {
                if (tile.Building == BuildingType.Factory) factories++;
                else if (tile.Building == BuildingType.CheapHouse) houses++;
                else if (tile.Building == BuildingType.Defense) defense++;
                else if (tile.Building == BuildingType.Research) hasResearch = true;
            }

            List<string> bonuses = new List<string>();

            if (factories > 0)
                bonuses.Add($"Factory x{factories} (+{factories * 4} resources)");
            if (houses > 0)
                bonuses.Add($"House x{houses} (+{houses} resources, +{houses} draws)");
            if (defense > 0)
                bonuses.Add($"Defense x{defense} (-{defense} disaster strength)");
            if (hasResearch)
                bonuses.Add("Research (draw twice)");

            if (bonuses.Count == 0)
                return "No network bonuses";

            return string.Join(", ", bonuses);
        }
    }
}

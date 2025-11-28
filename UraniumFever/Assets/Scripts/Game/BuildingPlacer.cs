using UnityEngine;
using UraniumFever.Core;

namespace UraniumFever.Game
{
    /// <summary>
    /// Result of attempting to place a building or bridge.
    /// </summary>
    public class PlacementResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static PlacementResult Succeed()
        {
            return new PlacementResult { Success = true, ErrorMessage = null };
        }

        public static PlacementResult Fail(string errorMessage)
        {
            return new PlacementResult { Success = false, ErrorMessage = errorMessage };
        }
    }

    /// <summary>
    /// Handles building and bridge placement logic with validation.
    /// Bridges are placed on edges between tiles.
    /// Buildings are placed on tiles that have adjacent bridges.
    /// </summary>
    public class BuildingPlacer
    {
        private GridManager _gridManager;

        public BuildingPlacer(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        /// <summary>
        /// Attempts to place a bridge on the edge between two tiles.
        /// </summary>
        public PlacementResult TryPlaceBridge(Vector2Int tile1, Vector2Int tile2, Player player)
        {
            // Check if tiles are adjacent
            GridEdge edge = _gridManager.GetEdge(tile1, tile2);
            if (edge == null)
            {
                return PlacementResult.Fail("Tiles are not adjacent or invalid");
            }

            // Check if bridge already exists
            if (edge.HasBridge)
            {
                return PlacementResult.Fail("Bridge already exists on this edge");
            }

            // Get bridge cost
            var cost = BuildingLibrary.GetCost(BuildingType.Bridge);

            // Check if player can afford
            if (!BuildingCostValidator.CanAfford(player, cost))
            {
                return PlacementResult.Fail("Cannot afford bridge");
            }

            // Check connectivity - bridge must connect to HQ or existing network
            if (!BridgeConnectivity.CanPlaceBridge(_gridManager, tile1, tile2, player.HQPosition))
            {
                return PlacementResult.Fail("Bridge must connect to your HQ or existing bridge network");
            }

            // All checks passed - deduct resources and place bridge
            BuildingCostValidator.DeductCost(player, cost);
            _gridManager.PlaceBridge(tile1, tile2, player.PlayerId);

            return PlacementResult.Succeed();
        }

        /// <summary>
        /// Attempts to place a building on a tile.
        /// </summary>
        public PlacementResult TryPlaceBuilding(BuildingType buildingType, Vector2Int position, Player player)
        {
            // Bridges must use TryPlaceBridge method
            if (buildingType == BuildingType.Bridge)
            {
                return PlacementResult.Fail("Use TryPlaceBridge method to place bridges on edges");
            }

            // Check if tile exists and is valid
            var tile = _gridManager.GetTile(position.x, position.y);
            if (tile == null)
            {
                return PlacementResult.Fail("Invalid tile position");
            }

            // Check if tile is already occupied
            if (tile.Building != null)
            {
                return PlacementResult.Fail("Tile is already occupied");
            }

            // Get building cost
            var cost = BuildingLibrary.GetCost(buildingType);

            // Check if player can afford
            if (!BuildingCostValidator.CanAfford(player, cost))
            {
                return PlacementResult.Fail("Cannot afford this building");
            }

            // Check connectivity - building must have adjacent bridge
            if (!BridgeConnectivity.CanPlaceBuilding(_gridManager, position))
            {
                return PlacementResult.Fail("Building must be adjacent to a bridge");
            }

            // All checks passed - deduct resources and place building
            BuildingCostValidator.DeductCost(player, cost);
            tile.Building = buildingType;
            tile.IsOccupied = true;

            return PlacementResult.Succeed();
        }

        /// <summary>
        /// Counts the number of a specific building type on the grid.
        /// </summary>
        public int GetBuildingCount(BuildingType buildingType)
        {
            int count = 0;
            var allTiles = _gridManager.GetAllTiles();

            foreach (var tile in allTiles)
            {
                if (tile.Building == buildingType)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Counts all buildings on the grid (excludes bridges which are on edges).
        /// </summary>
        public int GetTotalBuildingCount()
        {
            int count = 0;
            var allTiles = _gridManager.GetAllTiles();

            foreach (var tile in allTiles)
            {
                if (tile.Building != null)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Counts all house buildings.
        /// </summary>
        public int GetHouseCount()
        {
            return GetBuildingCount(BuildingType.CheapHouse);
        }

        /// <summary>
        /// Counts all bridges on the grid.
        /// </summary>
        public int GetBridgeCount()
        {
            int count = 0;
            var allEdges = _gridManager.GetAllEdges();

            foreach (var edge in allEdges)
            {
                if (edge.HasBridge)
                {
                    count++;
                }
            }

            return count;
        }
    }
}

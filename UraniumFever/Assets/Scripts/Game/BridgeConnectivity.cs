using UnityEngine;
using UraniumFever.Core;
using System.Collections.Generic;

namespace UraniumFever.Game
{
    /// <summary>
    /// Checks bridge connectivity rules for the edge-based bridge system.
    /// Bridges are placed on edges between tiles.
    /// Buildings can only be placed on tiles that have adjacent bridge edges.
    /// </summary>
    public static class BridgeConnectivity
    {
        /// <summary>
        /// Checks if a bridge can be placed on the edge between two tiles.
        /// Bridge must be adjacent to HQ or connect to an existing bridge network that reaches HQ.
        /// Bridges cannot be placed ON the HQ tile itself.
        /// </summary>
        public static bool CanPlaceBridge(GridManager gridManager, Vector2Int tile1, Vector2Int tile2, Vector2Int hqPosition)
        {
            // Bridges cannot be placed ON the HQ tile itself
            if (tile1 == hqPosition || tile2 == hqPosition)
                return false;

            // Check if either tile is adjacent to HQ (within HQ's influence zone)
            if (IsAdjacentTo(tile1, hqPosition) || IsAdjacentTo(tile2, hqPosition))
                return true;

            // Check if either tile is already connected to HQ via bridges
            if (IsConnectedToHQ(tile1, hqPosition, gridManager) ||
                IsConnectedToHQ(tile2, hqPosition, gridManager))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if two positions are adjacent (horizontally or vertically, not diagonally).
        /// </summary>
        private static bool IsAdjacentTo(Vector2Int pos1, Vector2Int pos2)
        {
            int dx = Mathf.Abs(pos1.x - pos2.x);
            int dy = Mathf.Abs(pos1.y - pos2.y);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        }

        /// <summary>
        /// Checks if a building can be placed on a tile.
        /// Buildings can only be placed on the 4 perpendicular (side) tiles of a bridge.
        /// Example: Bridge between (1,6)-(2,6) horizontal allows buildings on (1,5), (1,7), (2,5), (2,7) only.
        /// NOT on (1,6) or (2,6) which are the bridge tiles themselves.
        /// </summary>
        public static bool CanPlaceBuilding(GridManager gridManager, Vector2Int tilePos)
        {
            // Get all edges around ALL tiles in the grid and check if any bridge is adjacent perpendicular to tilePos
            GridEdge[] allEdges = gridManager.GetAllEdges();

            foreach (GridEdge edge in allEdges)
            {
                if (!edge.HasBridge) continue;

                // Check if tilePos is perpendicular (side) to this bridge
                // NOT on the bridge itself (Tile1 or Tile2)

                if (edge.IsHorizontal)
                {
                    // Horizontal bridge: buildings allowed above/below the bridge tiles
                    // Check if tilePos is directly above or below one of the bridge tiles
                    bool isAboveOrBelowTile1 = (tilePos.x == edge.Tile1Position.x) &&
                                                (tilePos.y == edge.Tile1Position.y + 1 || tilePos.y == edge.Tile1Position.y - 1);
                    bool isAboveOrBelowTile2 = (tilePos.x == edge.Tile2Position.x) &&
                                                (tilePos.y == edge.Tile2Position.y + 1 || tilePos.y == edge.Tile2Position.y - 1);

                    if (isAboveOrBelowTile1 || isAboveOrBelowTile2)
                        return true;
                }
                else
                {
                    // Vertical bridge: buildings allowed left/right of the bridge tiles
                    // Check if tilePos is directly left or right of one of the bridge tiles
                    bool isLeftOrRightTile1 = (tilePos.y == edge.Tile1Position.y) &&
                                               (tilePos.x == edge.Tile1Position.x + 1 || tilePos.x == edge.Tile1Position.x - 1);
                    bool isLeftOrRightTile2 = (tilePos.y == edge.Tile2Position.y) &&
                                               (tilePos.x == edge.Tile2Position.x + 1 || tilePos.x == edge.Tile2Position.x - 1);

                    if (isLeftOrRightTile1 || isLeftOrRightTile2)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a tile is connected to the HQ via a bridge network.
        /// Uses breadth-first search to traverse bridge connections.
        /// A network is connected if any tile in it is adjacent to the HQ.
        /// </summary>
        public static bool IsConnectedToHQ(Vector2Int tilePos, Vector2Int hqPosition, GridManager gridManager)
        {
            if (tilePos == hqPosition)
                return true;

            // Check if this tile is directly adjacent to HQ
            if (IsAdjacentTo(tilePos, hqPosition))
                return true;

            // BFS to find path from tile to HQ's influence zone via bridges
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(tilePos);
            visited.Add(tilePos);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                // Check if current tile is adjacent to HQ
                if (IsAdjacentTo(current, hqPosition))
                    return true;

                // Get all tiles adjacent via bridges
                Vector2Int[] adjacentTiles = gridManager.GetTilesAdjacentViaBridge(current);

                foreach (Vector2Int adjacent in adjacentTiles)
                {
                    // Check if adjacent tile is near HQ
                    if (IsAdjacentTo(adjacent, hqPosition))
                        return true;

                    if (!visited.Contains(adjacent))
                    {
                        visited.Add(adjacent);
                        queue.Enqueue(adjacent);
                    }
                }
            }

            return false; // No path to HQ's influence zone found
        }

        /// <summary>
        /// Gets all tiles in a player's connected network (all tiles reachable from HQ via bridges).
        /// Since bridges can't be on the HQ tile, we start from tiles adjacent to HQ.
        /// </summary>
        public static HashSet<Vector2Int> GetConnectedNetwork(Vector2Int hqPosition, GridManager gridManager)
        {
            HashSet<Vector2Int> network = new HashSet<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            // Add HQ to network
            network.Add(hqPosition);

            // Start BFS from tiles adjacent to HQ
            Vector2Int[] hqAdjacentPositions = new Vector2Int[]
            {
                new Vector2Int(hqPosition.x + 1, hqPosition.y),
                new Vector2Int(hqPosition.x - 1, hqPosition.y),
                new Vector2Int(hqPosition.x, hqPosition.y + 1),
                new Vector2Int(hqPosition.x, hqPosition.y - 1)
            };

            foreach (Vector2Int adjPos in hqAdjacentPositions)
            {
                // Check if this adjacent tile has any bridges
                GridEdge[] edges = gridManager.GetEdgesForTile(adjPos.x, adjPos.y);
                if (edges != null)
                {
                    foreach (GridEdge edge in edges)
                    {
                        if (edge.HasBridge)
                        {
                            // This tile is part of the network
                            if (!network.Contains(adjPos))
                            {
                                network.Add(adjPos);
                                queue.Enqueue(adjPos);
                            }
                            break;
                        }
                    }
                }
            }

            // Continue BFS from adjacent tiles
            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                // Get all tiles adjacent via bridges
                Vector2Int[] adjacentTiles = gridManager.GetTilesAdjacentViaBridge(current);

                foreach (Vector2Int adjacent in adjacentTiles)
                {
                    if (!network.Contains(adjacent))
                    {
                        network.Add(adjacent);
                        queue.Enqueue(adjacent);
                    }
                }
            }

            return network;
        }

        /// <summary>
        /// Gets all players whose networks are connected to the specified tile.
        /// Returns player IDs whose HQs can reach this tile via bridges.
        /// </summary>
        public static List<int> GetConnectedPlayers(Vector2Int tilePos, Vector2Int[] allHQPositions, int[] playerIds, GridManager gridManager)
        {
            List<int> connectedPlayers = new List<int>();

            for (int i = 0; i < allHQPositions.Length; i++)
            {
                if (IsConnectedToHQ(tilePos, allHQPositions[i], gridManager))
                {
                    connectedPlayers.Add(playerIds[i]);
                }
            }

            return connectedPlayers;
        }

        /// <summary>
        /// Checks if two players' networks are connected (share any bridge path).
        /// </summary>
        public static bool AreNetworksConnected(Vector2Int hq1, Vector2Int hq2, GridManager gridManager)
        {
            return IsConnectedToHQ(hq1, hq2, gridManager);
        }

        /// <summary>
        /// Gets all buildings in a connected network starting from HQ.
        /// </summary>
        public static List<GridTile> GetBuildingsInNetwork(Vector2Int hqPosition, GridManager gridManager)
        {
            List<GridTile> buildings = new List<GridTile>();
            HashSet<Vector2Int> network = GetConnectedNetwork(hqPosition, gridManager);

            foreach (Vector2Int tilePos in network)
            {
                GridTile tile = gridManager.GetTile(tilePos.x, tilePos.y);
                if (tile != null && tile.Building != null)
                {
                    buildings.Add(tile);
                }
            }

            return buildings;
        }
    }
}

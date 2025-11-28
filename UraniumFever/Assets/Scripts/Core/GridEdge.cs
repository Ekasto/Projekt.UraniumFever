using UnityEngine;

namespace UraniumFever.Core
{
    /// <summary>
    /// Represents an edge between two adjacent tiles on the grid.
    /// Bridges are placed on edges, not on tiles.
    /// </summary>
    public class GridEdge
    {
        public Vector2Int Tile1Position { get; private set; }
        public Vector2Int Tile2Position { get; private set; }
        public bool HasBridge { get; private set; }
        public int? OwnerId { get; private set; } // Null if no bridge, player ID if bridge placed
        public bool IsHorizontal { get; private set; }

        public GridEdge(Vector2Int tile1, Vector2Int tile2)
        {
            Tile1Position = tile1;
            Tile2Position = tile2;
            HasBridge = false;
            OwnerId = null;

            // Determine if horizontal or vertical
            IsHorizontal = tile1.y == tile2.y;
        }

        /// <summary>
        /// Places a bridge on this edge owned by the specified player.
        /// </summary>
        public void PlaceBridge(int playerId)
        {
            HasBridge = true;
            OwnerId = playerId;
        }

        /// <summary>
        /// Removes the bridge from this edge.
        /// </summary>
        public void RemoveBridge()
        {
            HasBridge = false;
            OwnerId = null;
        }

        /// <summary>
        /// Returns the two tile positions connected by this edge.
        /// </summary>
        public Vector2Int[] GetAdjacentTiles()
        {
            return new Vector2Int[] { Tile1Position, Tile2Position };
        }

        /// <summary>
        /// Gets the world position of the edge's midpoint for visualization.
        /// </summary>
        public Vector3 GetMidpoint(float tileSize, float tileSpacing)
        {
            float x1 = Tile1Position.x * (tileSize + tileSpacing);
            float z1 = Tile1Position.y * (tileSize + tileSpacing);
            float x2 = Tile2Position.x * (tileSize + tileSpacing);
            float z2 = Tile2Position.y * (tileSize + tileSpacing);

            return new Vector3((x1 + x2) / 2f, 0.5f, (z1 + z2) / 2f);
        }

        /// <summary>
        /// Gets the center position for a bridge that spans both tiles completely.
        /// Bridge should be positioned at the center of the combined area of both tiles.
        /// </summary>
        public Vector3 GetBridgeCenter(float tileSize, float tileSpacing)
        {
            // Calculate tile center positions (tile grid position * (size + spacing))
            float x1 = Tile1Position.x * (tileSize + tileSpacing);
            float z1 = Tile1Position.y * (tileSize + tileSpacing);
            float x2 = Tile2Position.x * (tileSize + tileSpacing);
            float z2 = Tile2Position.y * (tileSize + tileSpacing);

            // Bridge center is exactly at the midpoint between tile centers
            // This ensures the bridge spans across both tiles completely
            return new Vector3((x1 + x2) / 2f, 0.5f, (z1 + z2) / 2f);
        }

        /// <summary>
        /// Gets the rotation for visualizing this edge.
        /// </summary>
        public Quaternion GetRotation()
        {
            if (IsHorizontal)
            {
                return Quaternion.Euler(0, 90, 0); // Rotated to align with horizontal edge
            }
            else
            {
                return Quaternion.identity; // Vertical edge (default orientation)
            }
        }

        /// <summary>
        /// Checks if this edge connects the two specified tile positions.
        /// </summary>
        public bool ConnectsTiles(Vector2Int pos1, Vector2Int pos2)
        {
            return (Tile1Position == pos1 && Tile2Position == pos2) ||
                   (Tile1Position == pos2 && Tile2Position == pos1);
        }
    }
}

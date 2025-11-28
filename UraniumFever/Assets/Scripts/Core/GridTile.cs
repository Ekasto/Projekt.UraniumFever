using UnityEngine;

namespace UraniumFever.Core
{
    /// <summary>
    /// Represents a single tile in the game grid.
    /// </summary>
    public class GridTile
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public bool IsOccupied { get; set; }

        // Building placed on this tile (null if empty)
        public Game.BuildingType? Building { get; set; }

        public GridTile(int x, int y)
        {
            X = x;
            Y = y;
            WorldPosition = new Vector3(x, 0, y);
            IsOccupied = false;
            Building = null;
        }
    }
}

using UnityEngine;

namespace UraniumFever.Core
{
    /// <summary>
    /// Manages the game grid, creating and tracking tiles.
    /// </summary>
    public class GridManager
    {
        private GridTile[,] _grid;

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Creates a grid of the specified dimensions.
        /// </summary>
        public void CreateGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new GridTile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _grid[x, y] = new GridTile(x, y);
                }
            }
        }

        /// <summary>
        /// Gets a tile at the specified coordinates.
        /// </summary>
        /// <returns>The tile, or null if coordinates are invalid.</returns>
        public GridTile GetTile(int x, int y)
        {
            if (!IsValidPosition(x, y))
                return null;

            return _grid[x, y];
        }

        /// <summary>
        /// Checks if the given coordinates are within grid bounds.
        /// </summary>
        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// Returns all tiles in the grid.
        /// </summary>
        public GridTile[] GetAllTiles()
        {
            GridTile[] allTiles = new GridTile[Width * Height];
            int index = 0;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    allTiles[index] = _grid[x, y];
                    index++;
                }
            }

            return allTiles;
        }
    }
}

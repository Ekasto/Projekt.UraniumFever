using UnityEngine;

namespace UraniumFever.Core
{
    /// <summary>
    /// Manages the game grid, creating and tracking tiles and edges.
    /// Edges are where bridges are placed, connecting tiles.
    /// </summary>
    public class GridManager
    {
        private GridTile[,] _grid;
        private GridEdge[,] _horizontalEdges; // Edges between (x,y) and (x+1,y)
        private GridEdge[,] _verticalEdges;   // Edges between (x,y) and (x,y+1)

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Creates a grid of the specified dimensions, including tiles and edges.
        /// </summary>
        public void CreateGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new GridTile[width, height];

            // Create tiles
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _grid[x, y] = new GridTile(x, y);
                }
            }

            // Create horizontal edges (between tiles horizontally)
            _horizontalEdges = new GridEdge[width - 1, height];
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2Int tile1 = new Vector2Int(x, y);
                    Vector2Int tile2 = new Vector2Int(x + 1, y);
                    _horizontalEdges[x, y] = new GridEdge(tile1, tile2);
                }
            }

            // Create vertical edges (between tiles vertically)
            _verticalEdges = new GridEdge[width, height - 1];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Vector2Int tile1 = new Vector2Int(x, y);
                    Vector2Int tile2 = new Vector2Int(x, y + 1);
                    _verticalEdges[x, y] = new GridEdge(tile1, tile2);
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

        /// <summary>
        /// Gets the edge between two adjacent tiles.
        /// </summary>
        /// <returns>The edge, or null if tiles are not adjacent or invalid.</returns>
        public GridEdge GetEdge(Vector2Int tile1, Vector2Int tile2)
        {
            // Check if tiles are horizontally adjacent
            if (tile1.y == tile2.y && System.Math.Abs(tile1.x - tile2.x) == 1)
            {
                int minX = System.Math.Min(tile1.x, tile2.x);
                int y = tile1.y;
                if (minX >= 0 && minX < Width - 1 && y >= 0 && y < Height)
                {
                    return _horizontalEdges[minX, y];
                }
            }
            // Check if tiles are vertically adjacent
            else if (tile1.x == tile2.x && System.Math.Abs(tile1.y - tile2.y) == 1)
            {
                int x = tile1.x;
                int minY = System.Math.Min(tile1.y, tile2.y);
                if (x >= 0 && x < Width && minY >= 0 && minY < Height - 1)
                {
                    return _verticalEdges[x, minY];
                }
            }

            return null; // Not adjacent or invalid
        }

        /// <summary>
        /// Gets all edges surrounding a tile (up to 4 edges).
        /// </summary>
        public GridEdge[] GetEdgesForTile(int x, int y)
        {
            if (!IsValidPosition(x, y))
                return new GridEdge[0];

            System.Collections.Generic.List<GridEdge> edges = new System.Collections.Generic.List<GridEdge>();

            // Left edge (horizontal)
            if (x > 0)
                edges.Add(_horizontalEdges[x - 1, y]);

            // Right edge (horizontal)
            if (x < Width - 1)
                edges.Add(_horizontalEdges[x, y]);

            // Bottom edge (vertical)
            if (y > 0)
                edges.Add(_verticalEdges[x, y - 1]);

            // Top edge (vertical)
            if (y < Height - 1)
                edges.Add(_verticalEdges[x, y]);

            return edges.ToArray();
        }

        /// <summary>
        /// Places a bridge on the edge between two tiles.
        /// </summary>
        public bool PlaceBridge(Vector2Int tile1, Vector2Int tile2, int playerId)
        {
            GridEdge edge = GetEdge(tile1, tile2);
            if (edge == null)
                return false;

            edge.PlaceBridge(playerId);
            return true;
        }

        /// <summary>
        /// Returns all edges in the grid (both horizontal and vertical).
        /// </summary>
        public GridEdge[] GetAllEdges()
        {
            int totalEdges = (Width - 1) * Height + Width * (Height - 1);
            GridEdge[] allEdges = new GridEdge[totalEdges];
            int index = 0;

            // Add horizontal edges
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    allEdges[index] = _horizontalEdges[x, y];
                    index++;
                }
            }

            // Add vertical edges
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    allEdges[index] = _verticalEdges[x, y];
                    index++;
                }
            }

            return allEdges;
        }

        /// <summary>
        /// Gets all tiles that are adjacent to a tile via a bridge edge.
        /// </summary>
        public Vector2Int[] GetTilesAdjacentViaBridge(Vector2Int tilePos)
        {
            System.Collections.Generic.List<Vector2Int> adjacentTiles = new System.Collections.Generic.List<Vector2Int>();

            // Check all 4 edges around the tile
            GridEdge[] edges = GetEdgesForTile(tilePos.x, tilePos.y);
            foreach (GridEdge edge in edges)
            {
                if (edge.HasBridge)
                {
                    // Add the OTHER tile from this edge
                    Vector2Int[] tiles = edge.GetAdjacentTiles();
                    foreach (Vector2Int tile in tiles)
                    {
                        if (tile != tilePos)
                        {
                            adjacentTiles.Add(tile);
                        }
                    }
                }
            }

            return adjacentTiles.ToArray();
        }
    }
}

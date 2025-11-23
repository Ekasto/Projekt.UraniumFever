using NUnit.Framework;
using UraniumFever.Core;

namespace UraniumFever.Tests
{
    public class GridManagerTests
    {
        [Test]
        public void GridManager_CreateGrid_Creates8x8Grid()
        {
            // Arrange
            var gridManager = new GridManager();

            // Act
            gridManager.CreateGrid(8, 8);

            // Assert
            Assert.AreEqual(8, gridManager.Width);
            Assert.AreEqual(8, gridManager.Height);
        }

        [Test]
        public void GridManager_GetTile_ReturnsCorrectTile()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(8, 8);

            // Act
            var tile = gridManager.GetTile(3, 5);

            // Assert
            Assert.IsNotNull(tile);
            Assert.AreEqual(3, tile.X);
            Assert.AreEqual(5, tile.Y);
        }

        [Test]
        public void GridManager_GetTile_InvalidCoordinates_ReturnsNull()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(8, 8);

            // Act
            var tile1 = gridManager.GetTile(-1, 0);
            var tile2 = gridManager.GetTile(8, 0);
            var tile3 = gridManager.GetTile(0, -1);
            var tile4 = gridManager.GetTile(0, 8);

            // Assert
            Assert.IsNull(tile1);
            Assert.IsNull(tile2);
            Assert.IsNull(tile3);
            Assert.IsNull(tile4);
        }

        [Test]
        public void GridManager_IsValidPosition_ValidCoordinates_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(8, 8);

            // Act & Assert
            Assert.IsTrue(gridManager.IsValidPosition(0, 0));
            Assert.IsTrue(gridManager.IsValidPosition(7, 7));
            Assert.IsTrue(gridManager.IsValidPosition(3, 5));
        }

        [Test]
        public void GridManager_IsValidPosition_InvalidCoordinates_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(8, 8);

            // Act & Assert
            Assert.IsFalse(gridManager.IsValidPosition(-1, 0));
            Assert.IsFalse(gridManager.IsValidPosition(8, 0));
            Assert.IsFalse(gridManager.IsValidPosition(0, -1));
            Assert.IsFalse(gridManager.IsValidPosition(0, 8));
        }

        [Test]
        public void GridManager_GetAllTiles_ReturnsAll64Tiles()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(8, 8);

            // Act
            var allTiles = gridManager.GetAllTiles();

            // Assert
            Assert.AreEqual(64, allTiles.Length);
        }
    }
}

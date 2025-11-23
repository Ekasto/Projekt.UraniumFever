using NUnit.Framework;
using UnityEngine;
using UraniumFever.Core;

namespace UraniumFever.Tests
{
    public class GridTileTests
    {
        [Test]
        public void GridTile_Constructor_SetsPositionCorrectly()
        {
            // Arrange & Act
            var tile = new GridTile(3, 5);

            // Assert
            Assert.AreEqual(3, tile.X);
            Assert.AreEqual(5, tile.Y);
        }

        [Test]
        public void GridTile_Constructor_SetsWorldPositionCorrectly()
        {
            // Arrange & Act
            var tile = new GridTile(2, 4);

            // Assert
            Assert.AreEqual(new Vector3(2, 0, 4), tile.WorldPosition);
        }

        [Test]
        public void GridTile_IsOccupied_DefaultsToFalse()
        {
            // Arrange & Act
            var tile = new GridTile(0, 0);

            // Assert
            Assert.IsFalse(tile.IsOccupied);
        }

        [Test]
        public void GridTile_SetOccupied_UpdatesState()
        {
            // Arrange
            var tile = new GridTile(1, 1);

            // Act
            tile.IsOccupied = true;

            // Assert
            Assert.IsTrue(tile.IsOccupied);
        }
    }
}

using NUnit.Framework;
using UnityEngine;
using UraniumFever.Core;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class BuildingPlacerTests
    {
        [Test]
        public void TryPlaceBuilding_SuccessfulPlacement_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 6));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Electricity);

            var placer = new BuildingPlacer(gridManager);

            // Act
            var result = placer.TryPlaceBuilding(BuildingType.Factory, new Vector2Int(0, 6), player);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Food)); // 2 used
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Electricity)); // 1 used
            Assert.AreEqual(BuildingType.Factory, gridManager.GetTile(0, 6).Building);
        }

        [Test]
        public void TryPlaceBuilding_InsufficientResources_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 6));
            player.AddResource(ResourceType.Food); // Only 1, needs 2 for Factory

            var placer = new BuildingPlacer(gridManager);

            // Act
            var result = placer.TryPlaceBuilding(BuildingType.Factory, new Vector2Int(0, 6), player);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.ErrorMessage, Does.Contain("afford"));
        }

        [Test]
        public void TryPlaceBuilding_TileOccupied_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 6));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);

            var placer = new BuildingPlacer(gridManager);

            // Place first building
            placer.TryPlaceBuilding(BuildingType.Factory, new Vector2Int(0, 6), player);

            // Try to place second building on same tile
            var result = placer.TryPlaceBuilding(BuildingType.CheapHouse, new Vector2Int(0, 6), player);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.ErrorMessage, Does.Contain("occupied"));
        }

        [Test]
        public void TryPlaceBuilding_NotConnected_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 6));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);

            var placer = new BuildingPlacer(gridManager);

            // Try to place building far from HQ with no connection
            var result = placer.TryPlaceBuilding(BuildingType.Factory, new Vector2Int(5, 5), player);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.ErrorMessage, Does.Contain("connect"));
        }

        [Test]
        public void GetBuildingCount_CountsCorrectly()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 6));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);

            var placer = new BuildingPlacer(gridManager);

            // Place two cheap houses
            placer.TryPlaceBuilding(BuildingType.CheapHouse, new Vector2Int(0, 6), player);
            placer.TryPlaceBuilding(BuildingType.CheapHouse, new Vector2Int(1, 4), player);

            // Act
            var houseCount = placer.GetBuildingCount(BuildingType.CheapHouse);
            var factoryCount = placer.GetBuildingCount(BuildingType.Factory);

            // Assert
            Assert.AreEqual(2, houseCount);
            Assert.AreEqual(0, factoryCount);
        }
    }
}

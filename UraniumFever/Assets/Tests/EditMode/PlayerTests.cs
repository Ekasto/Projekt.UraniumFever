using NUnit.Framework;
using UnityEngine;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class PlayerTests
    {
        [Test]
        public void Player_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));

            // Assert
            Assert.AreEqual(1, player.PlayerId);
            Assert.AreEqual(ResourceType.Food, player.HQType);
            Assert.AreEqual(new Vector2Int(0, 4), player.HQPosition);
        }

        [Test]
        public void Player_Inventory_StartsEmpty()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));

            // Act
            var foodCount = player.GetResourceCount(ResourceType.Food);
            var electricityCount = player.GetResourceCount(ResourceType.Electricity);

            // Assert
            Assert.AreEqual(0, foodCount);
            Assert.AreEqual(0, electricityCount);
        }

        [Test]
        public void Player_AddResource_IncreasesCount()
        {
            // Arrange
            var player = new Player(2, ResourceType.Electricity, new Vector2Int(4, 0));

            // Act
            player.AddResource(ResourceType.Electricity);
            player.AddResource(ResourceType.Electricity);
            player.AddResource(ResourceType.Food);

            // Assert
            Assert.AreEqual(2, player.GetResourceCount(ResourceType.Electricity));
            Assert.AreEqual(1, player.GetResourceCount(ResourceType.Food));
        }

        [Test]
        public void Player_RemoveResource_DecreasesCount()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);

            // Act
            var removed = player.RemoveResource(ResourceType.Food, 2);

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(1, player.GetResourceCount(ResourceType.Food));
        }

        [Test]
        public void Player_RemoveResource_InsufficientAmount_ReturnsFalse()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            player.AddResource(ResourceType.Food);

            // Act
            var removed = player.RemoveResource(ResourceType.Food, 5);

            // Assert
            Assert.IsFalse(removed);
            Assert.AreEqual(1, player.GetResourceCount(ResourceType.Food));
        }

        [Test]
        public void Player_GetTotalResourceCount_ReturnsCorrectSum()
        {
            // Arrange
            var player = new Player(3, ResourceType.Medicine, new Vector2Int(7, 4));
            player.AddResource(ResourceType.Medicine);
            player.AddResource(ResourceType.Medicine);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.PlayerChoice);

            // Act
            var total = player.GetTotalResourceCount();

            // Assert
            Assert.AreEqual(4, total);
        }
    }
}

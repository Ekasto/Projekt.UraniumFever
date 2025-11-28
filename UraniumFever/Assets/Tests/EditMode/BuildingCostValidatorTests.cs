using NUnit.Framework;
using UnityEngine;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class BuildingCostValidatorTests
    {
        [Test]
        public void CanAfford_SufficientResources_ReturnsTrue()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Electricity);

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsTrue(canAfford);
        }

        [Test]
        public void CanAfford_InsufficientPrimary_ReturnsFalse()
        {
            // Arrange
            var player = new Player(2, ResourceType.Electricity, new Vector2Int(4, 0));
            player.AddResource(ResourceType.Electricity); // Only 1, needs 2
            player.AddResource(ResourceType.Food);

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsFalse(canAfford);
        }

        [Test]
        public void CanAfford_InsufficientSecondary_ReturnsFalse()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            // No secondary resources

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsFalse(canAfford);
        }

        [Test]
        public void CanAfford_PlayerChoiceAsPrimary_ReturnsTrue()
        {
            // Arrange
            var player = new Player(3, ResourceType.Medicine, new Vector2Int(7, 4));
            player.AddResource(ResourceType.Medicine); // 1 primary
            player.AddResource(ResourceType.PlayerChoice); // Can act as primary
            player.AddResource(ResourceType.Food); // 1 secondary

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsTrue(canAfford);
        }

        [Test]
        public void CanAfford_PlayerChoiceAsSecondary_ReturnsTrue()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.PlayerChoice); // Can act as secondary

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsTrue(canAfford);
        }

        [Test]
        public void CanAfford_MultiplePlayerChoice_DistributesCorrectly()
        {
            // Arrange
            var player = new Player(2, ResourceType.Electricity, new Vector2Int(4, 0));
            player.AddResource(ResourceType.Electricity); // 1 primary
            player.AddResource(ResourceType.PlayerChoice); // Acts as primary
            player.AddResource(ResourceType.PlayerChoice); // Acts as primary
            player.AddResource(ResourceType.PlayerChoice); // Acts as secondary

            var cost = new BuildingCost(primaryCount: 3, secondaryCount: 1);

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsTrue(canAfford);
        }

        [Test]
        public void CanAfford_FreeCost_AlwaysReturnsTrue()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            // No resources

            var cost = BuildingCost.Free();

            // Act
            var canAfford = BuildingCostValidator.CanAfford(player, cost);

            // Assert
            Assert.IsTrue(canAfford);
        }

        [Test]
        public void DeductCost_DeductsResourcesCorrectly()
        {
            // Arrange
            var player = new Player(1, ResourceType.Food, new Vector2Int(0, 4));
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Food);
            player.AddResource(ResourceType.Electricity);

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            BuildingCostValidator.DeductCost(player, cost);

            // Assert
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Food));
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Electricity));
        }

        [Test]
        public void DeductCost_UsesPlayerChoiceForPrimary()
        {
            // Arrange
            var player = new Player(3, ResourceType.Medicine, new Vector2Int(7, 4));
            player.AddResource(ResourceType.Medicine); // 1 primary
            player.AddResource(ResourceType.PlayerChoice); // Will be used as primary
            player.AddResource(ResourceType.Food); // Secondary

            var cost = new BuildingCost(primaryCount: 2, secondaryCount: 1);

            // Act
            BuildingCostValidator.DeductCost(player, cost);

            // Assert
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Medicine));
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.PlayerChoice));
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Food));
        }
    }
}

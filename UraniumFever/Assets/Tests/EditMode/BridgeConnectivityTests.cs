using NUnit.Framework;
using UnityEngine;
using UraniumFever.Core;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class BridgeConnectivityTests
    {
        [Test]
        public void CanPlaceBridge_FromHQ_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);
            var adjacentTile = new Vector2Int(1, 6);

            // Act - Bridge FROM HQ tile is not allowed (HQ tile cannot have bridges)
            var canPlace = BridgeConnectivity.CanPlaceBridge(gridManager, hqPosition, adjacentTile, hqPosition);

            // Assert
            Assert.IsFalse(canPlace);
        }

        [Test]
        public void CanPlaceBridge_AdjacentToHQ_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);
            var adjacentTile1 = new Vector2Int(1, 6);
            var adjacentTile2 = new Vector2Int(2, 6);

            // Act - Bridge adjacent to HQ (from tile next to HQ) should be allowed
            var canPlace = BridgeConnectivity.CanPlaceBridge(gridManager, adjacentTile1, adjacentTile2, hqPosition);

            // Assert
            Assert.IsTrue(canPlace);
        }

        [Test]
        public void CanPlaceBridge_NotConnectedToHQ_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);
            var tile1 = new Vector2Int(5, 5);
            var tile2 = new Vector2Int(5, 6);

            // Act - Bridge far from HQ with no connection should fail
            var canPlace = BridgeConnectivity.CanPlaceBridge(gridManager, tile1, tile2, hqPosition);

            // Assert
            Assert.IsFalse(canPlace);
        }

        [Test]
        public void CanPlaceBridge_ExtendingExistingNetwork_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);

            // Place first bridge adjacent to HQ (from tile next to HQ)
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);

            // Act - Try to extend network from tile (2,6) to (3,6)
            var canPlace = BridgeConnectivity.CanPlaceBridge(gridManager, new Vector2Int(2, 6), new Vector2Int(3, 6), hqPosition);

            // Assert
            Assert.IsTrue(canPlace);
        }

        [Test]
        public void CanPlaceBuilding_WithAdjacentBridge_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);

            // Place a bridge on edge between (1,6) and (2,6)
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);

            // Act - Try to place building on tile (1,6) which has adjacent bridge
            var canPlace = BridgeConnectivity.CanPlaceBuilding(gridManager, new Vector2Int(1, 6));

            // Assert
            Assert.IsTrue(canPlace);
        }

        [Test]
        public void CanPlaceBuilding_NoAdjacentBridge_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);

            // Act - Try to place building on tile with no adjacent bridges
            var canPlace = BridgeConnectivity.CanPlaceBuilding(gridManager, new Vector2Int(5, 5));

            // Assert
            Assert.IsFalse(canPlace);
        }

        [Test]
        public void CanPlaceBuilding_NextToTileWithBridge_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);

            // Place a bridge on edge between (1,6) and (2,6)
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);

            // Act - Try to place building on tile (1,7) which is adjacent to (1,6) that has a bridge
            var canPlace = BridgeConnectivity.CanPlaceBuilding(gridManager, new Vector2Int(1, 7));

            // Assert
            Assert.IsTrue(canPlace);
        }

        [Test]
        public void IsConnectedToHQ_DirectlyConnected_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);

            // Place bridge adjacent to HQ
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);

            // Act - Check if (1,6) is connected (it's adjacent to HQ via bridge network)
            var isConnected = BridgeConnectivity.IsConnectedToHQ(new Vector2Int(1, 6), hqPosition, gridManager);

            // Assert
            Assert.IsTrue(isConnected);
        }

        [Test]
        public void IsConnectedToHQ_NotConnected_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);

            // No bridges placed

            // Act
            var isConnected = BridgeConnectivity.IsConnectedToHQ(new Vector2Int(5, 5), hqPosition, gridManager);

            // Assert
            Assert.IsFalse(isConnected);
        }

        [Test]
        public void IsConnectedToHQ_MultipleHops_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);

            // Create a chain of bridges adjacent to HQ: (1,6) -> (2,6) -> (3,6) -> (4,6)
            // HQ at (0,6), so (1,6) is adjacent to HQ
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);
            gridManager.PlaceBridge(new Vector2Int(2, 6), new Vector2Int(3, 6), 1);
            gridManager.PlaceBridge(new Vector2Int(3, 6), new Vector2Int(4, 6), 1);

            // Act - Check if tile (4,6) is connected to HQ (through multiple hops)
            var isConnected = BridgeConnectivity.IsConnectedToHQ(new Vector2Int(4, 6), hqPosition, gridManager);

            // Assert
            Assert.IsTrue(isConnected);
        }

        [Test]
        public void GetConnectedNetwork_ReturnsAllConnectedTiles()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hqPosition = new Vector2Int(0, 6);

            // Create a network adjacent to HQ
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);
            gridManager.PlaceBridge(new Vector2Int(2, 6), new Vector2Int(3, 6), 1);

            // Act
            var network = BridgeConnectivity.GetConnectedNetwork(hqPosition, gridManager);

            // Assert
            Assert.IsTrue(network.Contains(new Vector2Int(0, 6))); // HQ
            Assert.IsTrue(network.Contains(new Vector2Int(1, 6))); // Adjacent to HQ, part of network
            Assert.IsTrue(network.Contains(new Vector2Int(2, 6))); // Connected via bridge
            Assert.IsTrue(network.Contains(new Vector2Int(3, 6))); // Connected via 2 bridges
            Assert.IsFalse(network.Contains(new Vector2Int(5, 5))); // Not connected
        }

        [Test]
        public void AreNetworksConnected_WhenConnected_ReturnsTrue()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hq1Position = new Vector2Int(0, 6);
            var hq2Position = new Vector2Int(4, 6);

            // Create bridges connecting both HQs' networks
            // HQ1 at (0,6), adjacent tile (1,6) has bridge
            // HQ2 at (4,6), adjacent tile (3,6) has bridge
            gridManager.PlaceBridge(new Vector2Int(1, 6), new Vector2Int(2, 6), 1);
            gridManager.PlaceBridge(new Vector2Int(2, 6), new Vector2Int(3, 6), 2);

            // Act
            var areConnected = BridgeConnectivity.AreNetworksConnected(hq1Position, hq2Position, gridManager);

            // Assert
            Assert.IsTrue(areConnected);
        }

        [Test]
        public void AreNetworksConnected_WhenNotConnected_ReturnsFalse()
        {
            // Arrange
            var gridManager = new GridManager();
            gridManager.CreateGrid(12, 12);
            var hq1Position = new Vector2Int(0, 6);
            var hq2Position = new Vector2Int(11, 6);

            // No bridges connecting them

            // Act
            var areConnected = BridgeConnectivity.AreNetworksConnected(hq1Position, hq2Position, gridManager);

            // Assert
            Assert.IsFalse(areConnected);
        }
    }
}

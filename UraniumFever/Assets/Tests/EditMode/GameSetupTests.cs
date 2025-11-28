using NUnit.Framework;
using UnityEngine;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class GameSetupTests
    {
        [Test]
        public void GameSetup_Creates3Players()
        {
            // Arrange
            var gameSetup = new GameSetup();

            // Act
            gameSetup.Initialize();

            // Assert
            Assert.AreEqual(3, gameSetup.Players.Length);
        }

        [Test]
        public void GameSetup_Player1_IsFoodAtPosition0_6()
        {
            // Arrange
            var gameSetup = new GameSetup();
            gameSetup.Initialize();

            // Act
            var player1 = gameSetup.Players[0];

            // Assert
            Assert.AreEqual(1, player1.PlayerId);
            Assert.AreEqual(ResourceType.Food, player1.HQType);
            Assert.AreEqual(new Vector2Int(0, 6), player1.HQPosition);
        }

        [Test]
        public void GameSetup_Player2_IsElectricityAtPosition6_0()
        {
            // Arrange
            var gameSetup = new GameSetup();
            gameSetup.Initialize();

            // Act
            var player2 = gameSetup.Players[1];

            // Assert
            Assert.AreEqual(2, player2.PlayerId);
            Assert.AreEqual(ResourceType.Electricity, player2.HQType);
            Assert.AreEqual(new Vector2Int(6, 0), player2.HQPosition);
        }

        [Test]
        public void GameSetup_Player3_IsMedicineAtPosition11_6()
        {
            // Arrange
            var gameSetup = new GameSetup();
            gameSetup.Initialize();

            // Act
            var player3 = gameSetup.Players[2];

            // Assert
            Assert.AreEqual(3, player3.PlayerId);
            Assert.AreEqual(ResourceType.Medicine, player3.HQType);
            Assert.AreEqual(new Vector2Int(11, 6), player3.HQPosition);
        }

        [Test]
        public void GameSetup_DeckIsInitialized()
        {
            // Arrange
            var gameSetup = new GameSetup();

            // Act
            gameSetup.Initialize();

            // Assert
            Assert.IsNotNull(gameSetup.Deck);
            Assert.Greater(gameSetup.Deck.TotalCards, 0);
        }

        [Test]
        public void GameSetup_RoundManagerIsInitialized()
        {
            // Arrange
            var gameSetup = new GameSetup();

            // Act
            gameSetup.Initialize();

            // Assert
            Assert.IsNotNull(gameSetup.RoundManager);
            Assert.AreEqual(1, gameSetup.RoundManager.CurrentRound);
        }

        [Test]
        public void GameSetup_GetCurrentPlayer_ReturnsCorrectPlayer()
        {
            // Arrange
            var gameSetup = new GameSetup();
            gameSetup.Initialize();

            // Act
            var currentPlayer = gameSetup.GetCurrentPlayer();

            // Assert
            Assert.AreEqual(1, currentPlayer.PlayerId);
        }
    }
}

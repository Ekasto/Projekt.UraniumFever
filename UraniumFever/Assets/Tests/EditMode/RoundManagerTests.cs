using NUnit.Framework;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class RoundManagerTests
    {
        [Test]
        public void RoundManager_StartsAtRound1()
        {
            // Arrange & Act
            var roundManager = new RoundManager();

            // Assert
            Assert.AreEqual(1, roundManager.CurrentRound);
        }

        [Test]
        public void RoundManager_IsSafeRound_Rounds1To3_ReturnsTrue()
        {
            // Arrange
            var roundManager = new RoundManager();

            // Act & Assert
            Assert.IsTrue(roundManager.IsSafeRound()); // Round 1

            roundManager.NextRound();
            Assert.IsTrue(roundManager.IsSafeRound()); // Round 2

            roundManager.NextRound();
            Assert.IsTrue(roundManager.IsSafeRound()); // Round 3
        }

        [Test]
        public void RoundManager_IsSafeRound_Round4AndUp_ReturnsFalse()
        {
            // Arrange
            var roundManager = new RoundManager();

            // Act - Go to round 4
            roundManager.NextRound();
            roundManager.NextRound();
            roundManager.NextRound();

            // Assert
            Assert.AreEqual(4, roundManager.CurrentRound);
            Assert.IsFalse(roundManager.IsSafeRound());
        }

        [Test]
        public void RoundManager_NextRound_IncrementsRound()
        {
            // Arrange
            var roundManager = new RoundManager();

            // Act
            roundManager.NextRound();
            roundManager.NextRound();

            // Assert
            Assert.AreEqual(3, roundManager.CurrentRound);
        }

        [Test]
        public void RoundManager_CurrentPlayerIndex_CyclesThrough3Players()
        {
            // Arrange
            var roundManager = new RoundManager();

            // Act & Assert
            Assert.AreEqual(0, roundManager.CurrentPlayerIndex); // Player 1

            roundManager.NextTurn();
            Assert.AreEqual(1, roundManager.CurrentPlayerIndex); // Player 2

            roundManager.NextTurn();
            Assert.AreEqual(2, roundManager.CurrentPlayerIndex); // Player 3

            roundManager.NextTurn();
            Assert.AreEqual(0, roundManager.CurrentPlayerIndex); // Back to Player 1
        }

        [Test]
        public void RoundManager_NextTurn_AfterAllPlayersGo_IncrementsRound()
        {
            // Arrange
            var roundManager = new RoundManager();

            // Act - All 3 players take their turns
            roundManager.NextTurn(); // Player 2's turn
            roundManager.NextTurn(); // Player 3's turn
            roundManager.NextTurn(); // Back to Player 1, new round

            // Assert
            Assert.AreEqual(2, roundManager.CurrentRound);
            Assert.AreEqual(0, roundManager.CurrentPlayerIndex);
        }
    }
}

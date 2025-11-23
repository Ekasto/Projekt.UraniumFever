using NUnit.Framework;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class DeckTests
    {
        [Test]
        public void Deck_Initialize_CreatesCards()
        {
            // Arrange
            var deck = new Deck();

            // Act
            deck.Initialize(resourceCardCount: 70, disasterCardCount: 30);

            // Assert
            Assert.AreEqual(100, deck.TotalCards);
            Assert.AreEqual(100, deck.RemainingCards);
        }

        [Test]
        public void Deck_DrawCard_ReturnsCard()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 10, disasterCardCount: 5);

            // Act
            var card = deck.DrawCard(isSafeRound: false);

            // Assert
            Assert.IsNotNull(card);
        }

        [Test]
        public void Deck_DrawCard_DecreasesRemainingCards()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 10, disasterCardCount: 5);

            // Act
            deck.DrawCard(isSafeRound: false);
            deck.DrawCard(isSafeRound: false);

            // Assert
            Assert.AreEqual(13, deck.RemainingCards);
        }

        [Test]
        public void Deck_DrawCard_SafeRound_OnlyReturnsResourceCards()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 20, disasterCardCount: 20);

            // Act - Draw many cards during safe round
            for (int i = 0; i < 15; i++)
            {
                var card = deck.DrawCard(isSafeRound: true);

                // Assert
                Assert.IsTrue(card.IsResource, $"Card {i} should be a resource card during safe round");
            }
        }

        [Test]
        public void Deck_DrawCard_NotSafeRound_CanReturnDisasterCards()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 5, disasterCardCount: 95);

            // Act - Draw many cards, at least one should be disaster
            bool foundDisaster = false;
            for (int i = 0; i < 20; i++)
            {
                var card = deck.DrawCard(isSafeRound: false);
                if (card.IsDisaster)
                {
                    foundDisaster = true;
                    break;
                }
            }

            // Assert
            Assert.IsTrue(foundDisaster, "Should be able to draw disaster cards when not in safe round");
        }

        [Test]
        public void Deck_DrawCard_WhenEmpty_ReshufflesDiscardPile()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 5, disasterCardCount: 0);

            // Act - Draw all cards
            for (int i = 0; i < 5; i++)
            {
                deck.DrawCard(isSafeRound: false);
            }

            // Deck should be empty, discard should have 5
            Assert.AreEqual(0, deck.RemainingCards);

            // Draw one more - should trigger reshuffle
            var card = deck.DrawCard(isSafeRound: false);

            // Assert
            Assert.IsNotNull(card);
            Assert.AreEqual(4, deck.RemainingCards); // 5 reshuffled - 1 drawn = 4
        }

        [Test]
        public void Deck_DiscardedCardCount_TracksCorrectly()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 10, disasterCardCount: 5);

            // Act
            deck.DrawCard(isSafeRound: false);
            deck.DrawCard(isSafeRound: false);
            deck.DrawCard(isSafeRound: false);

            // Assert
            Assert.AreEqual(3, deck.DiscardedCards);
            Assert.AreEqual(12, deck.RemainingCards);
        }

        [Test]
        public void Deck_ResourceDistribution_IsBalanced()
        {
            // Arrange
            var deck = new Deck();
            deck.Initialize(resourceCardCount: 40, disasterCardCount: 0);

            // Act - Count each resource type
            int[] resourceCounts = new int[4]; // 4 resource types
            for (int i = 0; i < 40; i++)
            {
                var card = deck.DrawCard(isSafeRound: false);
                if (card.IsResource)
                {
                    resourceCounts[(int)card.ResourceType.Value]++;
                }
            }

            // Assert - Each resource type should have roughly equal distribution (10 each)
            foreach (var count in resourceCounts)
            {
                Assert.GreaterOrEqual(count, 5); // At least 5 of each (accounting for randomness)
                Assert.LessOrEqual(count, 15); // At most 15 of each
            }
        }
    }
}

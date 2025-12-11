using NUnit.Framework;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class CardTests
    {
        [Test]
        public void Card_CreateResourceCard_SetsTypeCorrectly()
        {
            // Arrange & Act
            var card = Card.CreateResourceCard(ResourceType.Food, 5);

            // Assert
            Assert.AreEqual(CardType.Resource, card.Type);
            Assert.AreEqual(ResourceType.Food, card.ResourceType);
        }

        [Test]
        public void Card_CreateDisasterCard_SetsTypeCorrectly()
        {
            // Arrange & Act
            var card = Card.CreateDisasterCard(DisasterType.Earthquake);

            // Assert
            Assert.AreEqual(CardType.Disaster, card.Type);
            Assert.AreEqual(DisasterType.Earthquake, card.DisasterType);
        }

        [Test]
        public void Card_IsResource_ReturnsTrueForResourceCard()
        {
            // Arrange
            var card = Card.CreateResourceCard(ResourceType.Electricity, 3);

            // Act & Assert
            Assert.IsTrue(card.IsResource);
            Assert.IsFalse(card.IsDisaster);
        }

        [Test]
        public void Card_IsDisaster_ReturnsTrueForDisasterCard()
        {
            // Arrange
            var card = Card.CreateDisasterCard(DisasterType.Flood);

            // Act & Assert
            Assert.IsTrue(card.IsDisaster);
            Assert.IsFalse(card.IsResource);
        }

        [Test]
        public void Card_ToString_ResourceCard_ReturnsReadableString()
        {
            // Arrange
            var card = Card.CreateResourceCard(ResourceType.Medicine, 4);

            // Act
            var result = card.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Resource"));
            Assert.IsTrue(result.Contains("Medicine"));
        }

        [Test]
        public void Card_ResourceCard_HasCorrectValue()
        {
            // Arrange & Act
            var card = Card.CreateResourceCard(ResourceType.Food, 7);

            // Assert
            Assert.AreEqual(7, card.Value);
        }

        [Test]
        public void Card_DisasterCard_HasValueZero()
        {
            // Arrange & Act
            var card = Card.CreateDisasterCard(DisasterType.Tornado);

            // Assert
            Assert.AreEqual(0, card.Value);
        }

        [Test]
        public void Card_ToString_DisasterCard_ReturnsReadableString()
        {
            // Arrange
            var card = Card.CreateDisasterCard(DisasterType.Tornado);

            // Act
            var result = card.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Disaster"));
            Assert.IsTrue(result.Contains("Tornado"));
        }
    }
}

using NUnit.Framework;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class ResourceTypeTests
    {
        [Test]
        public void ResourceType_HasFourTypes()
        {
            // Arrange & Act
            var types = System.Enum.GetValues(typeof(ResourceType));

            // Assert
            Assert.AreEqual(4, types.Length);
        }

        [Test]
        public void ResourceType_ContainsElectricity()
        {
            // Act
            var hasElectricity = System.Enum.IsDefined(typeof(ResourceType), "Electricity");

            // Assert
            Assert.IsTrue(hasElectricity);
        }

        [Test]
        public void ResourceType_ContainsFood()
        {
            // Act
            var hasFood = System.Enum.IsDefined(typeof(ResourceType), "Food");

            // Assert
            Assert.IsTrue(hasFood);
        }

        [Test]
        public void ResourceType_ContainsMedicine()
        {
            // Act
            var hasMedicine = System.Enum.IsDefined(typeof(ResourceType), "Medicine");

            // Assert
            Assert.IsTrue(hasMedicine);
        }

        [Test]
        public void ResourceType_ContainsPlayerChoice()
        {
            // Act
            var hasPlayerChoice = System.Enum.IsDefined(typeof(ResourceType), "PlayerChoice");

            // Assert
            Assert.IsTrue(hasPlayerChoice);
        }
    }

    public class DisasterTypeTests
    {
        [Test]
        public void DisasterType_HasFiveTypes()
        {
            // Arrange & Act
            var types = System.Enum.GetValues(typeof(DisasterType));

            // Assert
            Assert.AreEqual(5, types.Length);
        }

        [Test]
        public void DisasterType_ContainsEarthquake()
        {
            // Act
            var hasEarthquake = System.Enum.IsDefined(typeof(DisasterType), "Earthquake");

            // Assert
            Assert.IsTrue(hasEarthquake);
        }

        [Test]
        public void DisasterType_ContainsFlood()
        {
            // Act
            var hasFlood = System.Enum.IsDefined(typeof(DisasterType), "Flood");

            // Assert
            Assert.IsTrue(hasFlood);
        }

        [Test]
        public void DisasterType_ContainsTornado()
        {
            // Act
            var hasTornado = System.Enum.IsDefined(typeof(DisasterType), "Tornado");

            // Assert
            Assert.IsTrue(hasTornado);
        }

        [Test]
        public void DisasterType_ContainsThief()
        {
            // Act
            var hasThief = System.Enum.IsDefined(typeof(DisasterType), "Thief");

            // Assert
            Assert.IsTrue(hasThief);
        }

        [Test]
        public void DisasterType_ContainsDonkey()
        {
            // Act
            var hasDonkey = System.Enum.IsDefined(typeof(DisasterType), "Donkey");

            // Assert
            Assert.IsTrue(hasDonkey);
        }
    }
}

using NUnit.Framework;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class BuildingTypeTests
    {
        [Test]
        public void BuildingType_HasEightTypes()
        {
            // Arrange & Act
            var types = System.Enum.GetValues(typeof(BuildingType));

            // Assert
            Assert.AreEqual(8, types.Length);
        }

        [Test]
        public void BuildingType_ContainsBridge()
        {
            // Act
            var hasBridge = System.Enum.IsDefined(typeof(BuildingType), "Bridge");

            // Assert
            Assert.IsTrue(hasBridge);
        }

        [Test]
        public void BuildingType_ContainsDefense()
        {
            // Act
            var hasDefense = System.Enum.IsDefined(typeof(BuildingType), "Defense");

            // Assert
            Assert.IsTrue(hasDefense);
        }

        [Test]
        public void BuildingType_ContainsFactory()
        {
            // Act
            var hasFactory = System.Enum.IsDefined(typeof(BuildingType), "Factory");

            // Assert
            Assert.IsTrue(hasFactory);
        }

        [Test]
        public void BuildingType_ContainsResearch()
        {
            // Act
            var hasResearch = System.Enum.IsDefined(typeof(BuildingType), "Research");

            // Assert
            Assert.IsTrue(hasResearch);
        }

        [Test]
        public void BuildingType_ContainsCheapHouse()
        {
            // Act
            var hasCheapHouse = System.Enum.IsDefined(typeof(BuildingType), "CheapHouse");

            // Assert
            Assert.IsTrue(hasCheapHouse);
        }

        [Test]
        public void BuildingType_ContainsCar()
        {
            // Act
            var hasCar = System.Enum.IsDefined(typeof(BuildingType), "Car");

            // Assert
            Assert.IsTrue(hasCar);
        }

        [Test]
        public void BuildingType_ContainsBrokenCar()
        {
            // Act
            var hasBrokenCar = System.Enum.IsDefined(typeof(BuildingType), "BrokenCar");

            // Assert
            Assert.IsTrue(hasBrokenCar);
        }

        [Test]
        public void BuildingType_ContainsHouseUpgrade()
        {
            // Act
            var hasHouseUpgrade = System.Enum.IsDefined(typeof(BuildingType), "HouseUpgrade");

            // Assert
            Assert.IsTrue(hasHouseUpgrade);
        }
    }

    public class BuildingCostTests
    {
        [Test]
        public void BuildingCost_Constructor_SetsPrimaryAndSecondary()
        {
            // Arrange & Act
            var cost = new BuildingCost(primaryCount: 3, secondaryCount: 2);

            // Assert
            Assert.AreEqual(3, cost.PrimaryCount);
            Assert.AreEqual(2, cost.SecondaryCount);
        }

        [Test]
        public void BuildingCost_TotalCost_ReturnsSum()
        {
            // Arrange
            var cost = new BuildingCost(primaryCount: 5, secondaryCount: 3);

            // Act
            var total = cost.TotalCost;

            // Assert
            Assert.AreEqual(8, total);
        }

        [Test]
        public void BuildingCost_FreeCost_HasZeroCosts()
        {
            // Arrange & Act
            var cost = BuildingCost.Free();

            // Assert
            Assert.AreEqual(0, cost.PrimaryCount);
            Assert.AreEqual(0, cost.SecondaryCount);
            Assert.AreEqual(0, cost.TotalCost);
        }
    }
}

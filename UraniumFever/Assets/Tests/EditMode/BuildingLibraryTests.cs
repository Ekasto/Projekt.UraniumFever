using NUnit.Framework;
using UraniumFever.Game;

namespace UraniumFever.Tests
{
    public class BuildingLibraryTests
    {
        [Test]
        public void BuildingLibrary_GetCost_Bridge_Returns2Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.Bridge);

            // Assert
            Assert.AreEqual(2, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(3, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_Defense_Returns7Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.Defense);

            // Assert
            Assert.AreEqual(7, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(8, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_Factory_Returns2Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.Factory);

            // Assert
            Assert.AreEqual(2, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(3, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_Research_Returns5Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.Research);

            // Assert
            Assert.AreEqual(5, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(6, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_CheapHouse_Returns2Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.CheapHouse);

            // Assert
            Assert.AreEqual(2, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(3, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_Car_ReturnsFree()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.Car);

            // Assert
            Assert.AreEqual(0, cost.PrimaryCount);
            Assert.AreEqual(0, cost.SecondaryCount);
            Assert.AreEqual(0, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_BrokenCar_Returns3Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.BrokenCar);

            // Assert
            Assert.AreEqual(3, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(4, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_GetCost_HouseUpgrade_Returns3Primary1Secondary()
        {
            // Act
            var cost = BuildingLibrary.GetCost(BuildingType.HouseUpgrade);

            // Assert
            Assert.AreEqual(3, cost.PrimaryCount);
            Assert.AreEqual(1, cost.SecondaryCount);
            Assert.AreEqual(4, cost.TotalCost);
        }

        [Test]
        public void BuildingLibrary_IsHouse_ReturnsTrueForHouses()
        {
            // Act & Assert
            Assert.IsTrue(BuildingLibrary.IsHouse(BuildingType.CheapHouse));
        }

        [Test]
        public void BuildingLibrary_IsHouse_ReturnsFalseForNonHouses()
        {
            // Act & Assert
            Assert.IsFalse(BuildingLibrary.IsHouse(BuildingType.Bridge));
            Assert.IsFalse(BuildingLibrary.IsHouse(BuildingType.Defense));
            Assert.IsFalse(BuildingLibrary.IsHouse(BuildingType.Factory));
        }

        [Test]
        public void BuildingLibrary_GetDisplayName_ReturnsReadableNames()
        {
            // Act
            var bridgeName = BuildingLibrary.GetDisplayName(BuildingType.Bridge);
            var defenseName = BuildingLibrary.GetDisplayName(BuildingType.Defense);

            // Assert
            Assert.IsNotEmpty(bridgeName);
            Assert.IsNotEmpty(defenseName);
        }
    }
}

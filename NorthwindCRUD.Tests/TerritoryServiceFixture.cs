using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class TerritoryServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateTerritory()
        {
            var territory = DataHelper.GetTerritory();

            var createdTerritory = DataHelper.CreateTerritory(territory);

            Assert.IsNotNull(createdTerritory);
            Assert.AreEqual(territory.TerritoryId, createdTerritory.TerritoryId);
            Assert.AreEqual(territory.TerritoryDescription, createdTerritory.TerritoryDescription);
            Assert.AreEqual(territory.RegionId, createdTerritory.RegionId);
        }

        [TestMethod]
        public void ShouldUpdateTerritory()
        {
            var territory = DataHelper.GetTerritory();
            var createdTerritory = DataHelper.CreateTerritory(territory);

            createdTerritory.TerritoryDescription = "Updated Territory";

            var updatedTerritory = DataHelper.TerritoryService.Update(createdTerritory);

            Assert.IsNotNull(updatedTerritory);
            Assert.AreEqual("Updated Territory", updatedTerritory.TerritoryDescription);
        }

        [TestMethod]
        public void ShouldDeleteTerritory()
        {
            var territory = DataHelper.CreateTerritory();
            DataHelper.TerritoryService.Delete(territory.TerritoryId);
            var deletedTerritory = DataHelper.TerritoryService.GetById(territory.TerritoryId);

            Assert.IsNull(deletedTerritory);
        }

        [TestMethod]
        public void ShouldReturnAllTerritories()
        {
            DataHelper.CreateTerritory();
            DataHelper.CreateTerritory();

            var result = DataHelper.TerritoryService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldReturnTerritory()
        {
            var territory = DataHelper.CreateTerritory();

            var result = DataHelper.TerritoryService.GetById(territory.TerritoryId);

            Assert.IsNotNull(result);
            Assert.AreEqual(territory.TerritoryId, result.TerritoryId);
            Assert.AreEqual(territory.TerritoryDescription, result.TerritoryDescription);
            Assert.AreEqual(territory.RegionId, result.RegionId);
        }
    }
}
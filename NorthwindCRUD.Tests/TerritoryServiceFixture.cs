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
            createdTerritory = DataHelper2.TerritoryService.GetById(createdTerritory.TerritoryId);
            Assert.IsNotNull(createdTerritory);
            Assert.AreEqual(territory.TerritoryId, createdTerritory.TerritoryId);
            Assert.AreEqual(territory.TerritoryDescription, createdTerritory.TerritoryDescription);
            Assert.AreEqual(territory.RegionId, createdTerritory.RegionId);
        }

        [TestMethod]
        public void ShouldUpdateTerritory()
        {
            var territory = DataHelper.GetTerritory();
            string originalTerritoryDescription = territory.TerritoryDescription;
            var createdTerritory = DataHelper.CreateTerritory(territory);

            createdTerritory.TerritoryDescription = "Updated Territory";

            var updatedTerritory = DataHelper.TerritoryService.Update(createdTerritory.TerritoryId, createdTerritory);

            Assert.IsNotNull(updatedTerritory);
            updatedTerritory = DataHelper2.TerritoryService.GetById(updatedTerritory.TerritoryId);
            Assert.IsNotNull(updatedTerritory);
            Assert.AreNotEqual(originalTerritoryDescription, updatedTerritory.TerritoryDescription);
            Assert.AreEqual(createdTerritory.TerritoryDescription, updatedTerritory.TerritoryDescription);
        }

        [TestMethod]
        public void ShouldDeleteTerritory()
        {
            var territory = DataHelper.CreateTerritory();
            DataHelper.TerritoryService.Delete(territory.TerritoryId);
            var deletedTerritory = DataHelper2.TerritoryService.GetById(territory.TerritoryId);

            Assert.IsNull(deletedTerritory);
        }

        [TestMethod]
        public void ShouldReturnAllTerritories()
        {
            DataHelper.CreateTerritory();
            DataHelper.CreateTerritory();

            var result = DataHelper2.TerritoryService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldReturnTerritory()
        {
            var territory = DataHelper.CreateTerritory();

            var result = DataHelper2.TerritoryService.GetById(territory.TerritoryId);

            Assert.IsNotNull(result);
            Assert.AreEqual(territory.TerritoryId, result.TerritoryId);
            Assert.AreEqual(territory.TerritoryDescription, result.TerritoryDescription);
            Assert.AreEqual(territory.RegionId, result.RegionId);
        }
    }
}
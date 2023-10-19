using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class RegionServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateRegion()
        {
            var region = DataHelper.GetRegion();

            var createdRegion = DataHelper.RegionService.Create(region);

            Assert.IsNotNull(createdRegion);
            createdRegion = DataHelper2.RegionService.GetById(createdRegion.RegionId);
            Assert.AreEqual(region.RegionId, createdRegion.RegionId);
            Assert.AreEqual(region.RegionDescription, createdRegion.RegionDescription);
        }

        [TestMethod]
        public void ShouldUpdateRegion()
        {
            var region = DataHelper.CreateRegion();
            string originalRegionDescription = region.RegionDescription;
            region.RegionDescription = "Updated Region";

            var updatedRegion = DataHelper.RegionService.Update(region);

            Assert.IsNotNull(updatedRegion);
            updatedRegion = DataHelper2.RegionService.GetById(updatedRegion.RegionId);
            Assert.AreNotEqual(originalRegionDescription, updatedRegion.RegionDescription);
            Assert.AreEqual(region.RegionDescription, updatedRegion.RegionDescription);
        }

        [TestMethod]
        public void ShouldDeleteRegion()
        {
            var region = DataHelper.CreateRegion();

            DataHelper.RegionService.Delete(region.RegionId);
            var deletedRegion = DataHelper2.RegionService.GetById(region.RegionId);

            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public void ShouldGetAllRegions()
        {
            DataHelper.CreateRegion();
            DataHelper.CreateRegion();

            var result = DataHelper2.RegionService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetByRegionId()
        {
            var region = DataHelper.CreateRegion();

            var result = DataHelper2.RegionService.GetById(region.RegionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(region.RegionId, result.RegionId);
            Assert.AreEqual(region.RegionDescription, result.RegionDescription);
        }
    }
}
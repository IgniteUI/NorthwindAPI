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
            Assert.AreEqual(region.RegionId, createdRegion.RegionId);
            Assert.AreEqual(region.RegionDescription, createdRegion.RegionDescription);
        }

        [TestMethod]
        public void ShouldUpdateRegion()
        {
            var region = DataHelper.CreateRegion();

            region.RegionDescription = "Updated Region";

            var updatedRegion = DataHelper.RegionService.Update(region);

            Assert.IsNotNull(updatedRegion);
            Assert.AreEqual("Updated Region", updatedRegion.RegionDescription);
        }

        [TestMethod]
        public void ShouldDeleteRegion()
        {
            var region = DataHelper.CreateRegion();

            DataHelper.RegionService.Delete(region.RegionId);
            var deletedRegion = DataHelper.RegionService.GetById(region.RegionId);

            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public void ShouldGetAllRegions()
        {
            DataHelper.CreateRegion();
            DataHelper.CreateRegion();

            var result = DataHelper.RegionService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetByRegionId()
        {
            var region = DataHelper.CreateRegion();

            var result = DataHelper.RegionService.GetById(region.RegionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(region.RegionId, result.RegionId);
            Assert.AreEqual(region.RegionDescription, result.RegionDescription);
        }
    }
}
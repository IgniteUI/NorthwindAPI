using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class RegionServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateRegion()
        {
            var region = DataHelper.GetRegion();

            var createdRegion = await DataHelper.RegionService.Create(region);

            Assert.IsNotNull(createdRegion);
            createdRegion = DataHelper2.RegionService.GetById(createdRegion.RegionId);
            Assert.IsNotNull(createdRegion);
            Assert.AreEqual(region.RegionDescription, createdRegion.RegionDescription);
        }

        [TestMethod]
        public async Task ShouldUpdateRegion()
        {
            var region = await DataHelper.CreateRegion();
            string originalRegionDescription = region.RegionDescription;
            region.RegionDescription = "Updated Region";

            var updatedRegion = await DataHelper.RegionService.Update(region, region.RegionId);

            Assert.IsNotNull(updatedRegion);
            updatedRegion = DataHelper2.RegionService.GetById(updatedRegion.RegionId);
            Assert.IsNotNull(updatedRegion);
            Assert.AreNotEqual(originalRegionDescription, updatedRegion.RegionDescription);
            Assert.AreEqual(region.RegionDescription, updatedRegion.RegionDescription);
        }

        [TestMethod]
        public async Task ShouldDeleteRegion()
        {
            var region = await DataHelper.CreateRegion();

            DataHelper.RegionService.Delete(region.RegionId);
            var deletedRegion = DataHelper2.RegionService.GetById(region.RegionId);

            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public async Task ShouldGetAllRegions()
        {
            await DataHelper.CreateRegion();
            await DataHelper.CreateRegion();

            var result = DataHelper2.RegionService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public async Task ShouldGetByRegionId()
        {
            var region = await DataHelper.CreateRegion();

            var result = DataHelper2.RegionService.GetById(region.RegionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(region.RegionId, result.RegionId);
            Assert.AreEqual(region.RegionDescription, result.RegionDescription);
        }
    }
}
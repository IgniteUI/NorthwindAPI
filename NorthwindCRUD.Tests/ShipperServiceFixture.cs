using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class ShipperServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateShipper()
        {
            var shipper = DataHelper.GetShipper();

            var createdShipper = await DataHelper.ShipperService.Create(shipper);

            Assert.IsNotNull(createdShipper);
            createdShipper = DataHelper2.ShipperService.GetById(createdShipper.ShipperId);
            Assert.IsNotNull(createdShipper);
            Assert.AreEqual(shipper.CompanyName, createdShipper.CompanyName);
            Assert.AreEqual(shipper.Phone, createdShipper.Phone);
        }

        [TestMethod]
        public async Task ShouldUpdateShipper()
        {
            var shipper = DataHelper.GetShipper();
            string originalCompanyName = shipper.CompanyName;
            string originalPhone = shipper.Phone;
            var insertedShipper = await DataHelper.ShipperService.Create(shipper);

            shipper.CompanyName = "Updated shipper company";
            shipper.Phone = "555-555-5555";

            var updatedShipper = await DataHelper.ShipperService.Update(shipper, insertedShipper.ShipperId);

            Assert.IsNotNull(updatedShipper);
            updatedShipper = DataHelper2.ShipperService.GetById(updatedShipper.ShipperId);
            Assert.IsNotNull(updatedShipper);
            Assert.AreNotEqual(originalCompanyName, updatedShipper.CompanyName);
            Assert.AreNotEqual(originalPhone, updatedShipper.Phone);
            Assert.AreEqual(shipper.CompanyName, updatedShipper.CompanyName);
            Assert.AreEqual(shipper.Phone, updatedShipper.Phone);
        }

        [TestMethod]
        public async Task ShouldDeleteShipper()
        {
            var shipper = DataHelper.GetShipper();
            await DataHelper.ShipperService.Create(shipper);

            DataHelper.ShipperService.Delete(shipper.ShipperId);
            var deletedShipper = DataHelper2.ShipperService.GetById(shipper.ShipperId);

            Assert.IsNull(deletedShipper);
        }

        [TestMethod]
        public async Task ShouldGetAllShippers()
        {
            await DataHelper.CreateShipper();
            await DataHelper.CreateShipper();

            var result = DataHelper2.ShipperService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public async Task ShouldGetShipperById()
        {
            var shipper = DataHelper.GetShipper();
            var insertedShipper = await DataHelper.ShipperService.Create(shipper);

            var result = DataHelper2.ShipperService.GetById(insertedShipper.ShipperId);

            Assert.IsNotNull(result);
            Assert.AreEqual(insertedShipper.ShipperId, result.ShipperId);
            Assert.AreEqual(shipper.CompanyName, result.CompanyName);
            Assert.AreEqual(shipper.Phone, result.Phone);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class ShipperServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateShipper()
        {
            var shipper = DataHelper.GetShipper();

            var createdShipper = DataHelper.ShipperService.Create(shipper);

            Assert.IsNotNull(createdShipper);
            Assert.AreEqual(shipper.ShipperId, createdShipper.ShipperId);
            Assert.AreEqual(shipper.CompanyName, createdShipper.CompanyName);
            Assert.AreEqual(shipper.Phone, createdShipper.Phone);
        }

        [TestMethod]
        public void ShouldUpdateShipper()
        {
            var shipper = DataHelper.GetShipper();
            DataHelper.ShipperService.Create(shipper);

            shipper.CompanyName = "Updated shipper company";
            shipper.Phone = "555-555-5555";

            var updatedShipper = DataHelper.ShipperService.Update(shipper);

            Assert.IsNotNull(updatedShipper);
            Assert.AreEqual("Updated shipper company", updatedShipper.CompanyName);
            Assert.AreEqual("555-555-5555", updatedShipper.Phone);
        }

        [TestMethod]
        public void ShouldDeleteShipper()
        {
            var shipper = DataHelper.GetShipper();
            DataHelper.ShipperService.Create(shipper);

            DataHelper.ShipperService.Delete(shipper.ShipperId);
            var deletedShipper = DataHelper.ShipperService.GetById(shipper.ShipperId);

            Assert.IsNull(deletedShipper);
        }

        [TestMethod]
        public void ShouldGetAllShippers()
        {
            DataHelper.CreateShipper();
            DataHelper.CreateShipper();

            var result = DataHelper.ShipperService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetShipperById()
        {
            var shipper = DataHelper.GetShipper();
            DataHelper.ShipperService.Create(shipper);

            var result = DataHelper.ShipperService.GetById(shipper.ShipperId);

            Assert.IsNotNull(result);
            Assert.AreEqual(shipper.ShipperId, result.ShipperId);
            Assert.AreEqual(shipper.CompanyName, result.CompanyName);
            Assert.AreEqual(shipper.Phone, result.Phone);
        }
    }
}
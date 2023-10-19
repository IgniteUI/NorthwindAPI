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
            createdShipper = DataHelper2.ShipperService.GetById(createdShipper.ShipperId);
            Assert.AreEqual(shipper.ShipperId, createdShipper.ShipperId);
            Assert.AreEqual(shipper.CompanyName, createdShipper.CompanyName);
            Assert.AreEqual(shipper.Phone, createdShipper.Phone);
        }

        [TestMethod]
        public void ShouldUpdateShipper()
        {
            var shipper = DataHelper.GetShipper();
            string originalCompanyName = shipper.CompanyName;
            string originalPhone = shipper.Phone;
            DataHelper.ShipperService.Create(shipper);

            shipper.CompanyName = "Updated shipper company";
            shipper.Phone = "555-555-5555";

            var updatedShipper = DataHelper.ShipperService.Update(shipper);

            Assert.IsNotNull(updatedShipper);
            updatedShipper = DataHelper2.ShipperService.GetById(updatedShipper.ShipperId);
            Assert.AreNotEqual(originalCompanyName, updatedShipper.CompanyName);
            Assert.AreNotEqual(originalPhone, updatedShipper.Phone);
            Assert.AreEqual(shipper.CompanyName, updatedShipper.CompanyName);
            Assert.AreEqual(shipper.Phone, updatedShipper.Phone);
        }

        [TestMethod]
        public void ShouldDeleteShipper()
        {
            var shipper = DataHelper.GetShipper();
            DataHelper.ShipperService.Create(shipper);

            DataHelper.ShipperService.Delete(shipper.ShipperId);
            var deletedShipper = DataHelper2.ShipperService.GetById(shipper.ShipperId);

            Assert.IsNull(deletedShipper);
        }

        [TestMethod]
        public void ShouldGetAllShippers()
        {
            DataHelper.CreateShipper();
            DataHelper.CreateShipper();

            var result = DataHelper2.ShipperService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetShipperById()
        {
            var shipper = DataHelper.GetShipper();
            DataHelper.ShipperService.Create(shipper);

            var result = DataHelper2.ShipperService.GetById(shipper.ShipperId);

            Assert.IsNotNull(result);
            Assert.AreEqual(shipper.ShipperId, result.ShipperId);
            Assert.AreEqual(shipper.CompanyName, result.CompanyName);
            Assert.AreEqual(shipper.Phone, result.Phone);
        }
    }
}
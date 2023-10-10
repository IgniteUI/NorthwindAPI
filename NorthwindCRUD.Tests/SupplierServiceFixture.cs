using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class SupplierServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateSupplier()
        {
            var supplier = DataHelper.GetSupplier();

            var createdSupplier = DataHelper.SupplierService.Create(supplier);

            Assert.IsNotNull(createdSupplier);
            Assert.AreEqual(supplier.SupplierId, createdSupplier.SupplierId);
            Assert.AreEqual(supplier.CompanyName, createdSupplier.CompanyName);
            Assert.AreEqual(supplier.ContactName, createdSupplier.ContactName);
        }

        [TestMethod]
        public void ShouldUpdateSupplier()
        {
            var supplier = DataHelper.GetSupplier();
            DataHelper.SupplierService.Create(supplier);

            supplier.CompanyName = "Updated Supplier";
            supplier.ContactName = "Updated Contact";
            supplier.ContactTitle = "Updated Title";

            var updatedSupplier = DataHelper.SupplierService.Update(supplier);

            Assert.IsNotNull(updatedSupplier);
            Assert.AreEqual("Updated Supplier", updatedSupplier.CompanyName);
            Assert.AreEqual("Updated Contact", updatedSupplier.ContactName);
            Assert.AreEqual("Updated Title", updatedSupplier.ContactTitle);
        }

        [TestMethod]
        public void ShouldDeleteSupplier()
        {
            var supplier = DataHelper.GetSupplier();
            DataHelper.SupplierService.Create(supplier);

            DataHelper.SupplierService.Delete(supplier.SupplierId);
            var deletedSupplier = DataHelper.SupplierService.GetById(supplier.SupplierId);

            Assert.IsNull(deletedSupplier);
        }

        [TestMethod]
        public void ShouldGetAllSuppliers()
        {
            DataHelper.CreateSupplier();
            DataHelper.CreateSupplier();

            var result = DataHelper.SupplierService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetBySupplierId()
        {
            var supplier = DataHelper.GetSupplier();
            DataHelper.SupplierService.Create(supplier);

            var result = DataHelper.SupplierService.GetById(supplier.SupplierId);

            Assert.IsNotNull(result);
            Assert.AreEqual(supplier.SupplierId, result.SupplierId);
            Assert.AreEqual(supplier.CompanyName, result.CompanyName);
            Assert.AreEqual(supplier.ContactName, result.ContactName);
        }
    }
}
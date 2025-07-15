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
            createdSupplier = DataHelper2.SupplierService.GetById(createdSupplier.SupplierId);
            Assert.IsNotNull(createdSupplier);
            Assert.AreEqual(supplier.SupplierId, createdSupplier.SupplierId);
            Assert.AreEqual(supplier.CompanyName, createdSupplier.CompanyName);
            Assert.AreEqual(supplier.ContactName, createdSupplier.ContactName);
        }

        [TestMethod]
        public void ShouldUpdateSupplier()
        {
            var supplier = DataHelper.GetSupplier();
            string? originalCompanyName = supplier.CompanyName;
            string? originalContactName = supplier.ContactName;
            string? originalContactTitle = supplier.ContactTitle;
            DataHelper.SupplierService.Create(supplier);

            supplier.CompanyName = "Updated Supplier";
            supplier.ContactName = "Updated Contact";
            supplier.ContactTitle = "Updated Title";

            var updatedSupplier = DataHelper.SupplierService.Update(supplier.SupplierId, supplier);
            Assert.IsNotNull(updatedSupplier);
            updatedSupplier = DataHelper2.SupplierService.GetById(updatedSupplier.SupplierId);
            Assert.IsNotNull(updatedSupplier);
            Assert.AreEqual(supplier.CompanyName, updatedSupplier.CompanyName);
            Assert.AreEqual(supplier.ContactName, updatedSupplier.ContactName);
            Assert.AreEqual(supplier.ContactTitle, updatedSupplier.ContactTitle);
        }

        [TestMethod]
        public void ShouldDeleteSupplier()
        {
            var supplier = DataHelper.GetSupplier();
            DataHelper.SupplierService.Create(supplier);

            DataHelper.SupplierService.Delete(supplier.SupplierId);
            var deletedSupplier = DataHelper2.SupplierService.GetById(supplier.SupplierId);

            Assert.IsNull(deletedSupplier);
        }

        [TestMethod]
        public void ShouldGetAllSuppliers()
        {
            DataHelper.CreateSupplier();
            DataHelper.CreateSupplier();

            var result = DataHelper2.SupplierService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetBySupplierId()
        {
            var supplier = DataHelper.GetSupplier();
            DataHelper.SupplierService.Create(supplier);

            var result = DataHelper2.SupplierService.GetById(supplier.SupplierId);

            Assert.IsNotNull(result);
            Assert.AreEqual(supplier.SupplierId, result.SupplierId);
            Assert.AreEqual(supplier.CompanyName, result.CompanyName);
            Assert.AreEqual(supplier.ContactName, result.ContactName);
        }
    }
}
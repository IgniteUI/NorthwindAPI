using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class SupplierServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateSupplier()
        {
            var supplier = DataHelper.GetSupplier();

            var createdSupplier = await DataHelper.SupplierService.Create(supplier);

            Assert.IsNotNull(createdSupplier);
            createdSupplier = DataHelper2.SupplierService.GetById(createdSupplier.SupplierId);
            Assert.IsNotNull(createdSupplier);
            Assert.AreEqual(supplier.CompanyName, createdSupplier.CompanyName);
            Assert.AreEqual(supplier.ContactName, createdSupplier.ContactName);
        }

        [TestMethod]
        public async Task ShouldUpdateSupplier()
        {
            var supplier = DataHelper.GetSupplier();
            string? originalCompanyName = supplier.CompanyName;
            string? originalContactName = supplier.ContactName;
            string? originalContactTitle = supplier.ContactTitle;
            var insertedSupplier = await DataHelper.SupplierService.Create(supplier);

            insertedSupplier.CompanyName = "Updated Supplier";
            insertedSupplier.ContactName = "Updated Contact";
            insertedSupplier.ContactTitle = "Updated Title";

            var updatedSupplier = await DataHelper.SupplierService.Update(insertedSupplier, insertedSupplier.SupplierId);
            Assert.IsNotNull(updatedSupplier);
            updatedSupplier = DataHelper2.SupplierService.GetById(updatedSupplier.SupplierId);
            Assert.IsNotNull(updatedSupplier);
            Assert.AreEqual(insertedSupplier.CompanyName, updatedSupplier.CompanyName);
            Assert.AreEqual(insertedSupplier.ContactName, updatedSupplier.ContactName);
            Assert.AreEqual(insertedSupplier.ContactTitle, updatedSupplier.ContactTitle);
        }

        [TestMethod]
        public async Task ShouldDeleteSupplier()
        {
            var supplier = DataHelper.GetSupplier();
            await DataHelper.SupplierService.Create(supplier);

            DataHelper.SupplierService.Delete(supplier.SupplierId);
            var deletedSupplier = DataHelper2.SupplierService.GetById(supplier.SupplierId);

            Assert.IsNull(deletedSupplier);
        }

        [TestMethod]
        public async Task ShouldGetAllSuppliers()
        {
            await DataHelper.CreateSupplier();
            await DataHelper.CreateSupplier();

            var result = DataHelper2.SupplierService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public async Task ShouldGetBySupplierId()
        {
            var supplier = DataHelper.GetSupplier();
            var insertedSupplier = await DataHelper.SupplierService.Create(supplier);

            var result = DataHelper2.SupplierService.GetById(insertedSupplier.SupplierId);

            Assert.IsNotNull(result);
            Assert.AreEqual(supplier.CompanyName, result.CompanyName);
            Assert.AreEqual(supplier.ContactName, result.ContactName);
        }
    }
}
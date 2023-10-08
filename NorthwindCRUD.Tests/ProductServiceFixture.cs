using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Services;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class ProductServiceFixture : BaseFixture
    {
        private ProductService productService;
        private CategoryService categoryService;
        private SupplierService supplierService;

        [TestInitialize]
        public void Initialize()
        {
            DataContext context = GetInMemoryDatabaseContext();
            productService = new ProductService(context);
            categoryService = new CategoryService(context);
            supplierService = new SupplierService(context);
        }

        [TestMethod]
        public void ShouldCreateProduct()
        {
            var categoryId = categoryService.GetAll().GetRandomElement().CategoryId;
            var supplierId = supplierService.GetAll().GetRandomElement().SupplierId;
            var product = new ProductDb
            {
                UnitPrice = 10,
                UnitsInStock = 100,
                CategoryId = categoryId,
                SupplierId = supplierId,
            };

            var createdProduct = productService.Create(product);

            Assert.IsNotNull(createdProduct);
            Assert.AreEqual(product.UnitPrice, createdProduct.UnitPrice);
            Assert.AreEqual(product.UnitsInStock, createdProduct.UnitsInStock);
        }

        [TestMethod]
        public void ShouldUpdateProduct()
        {
            var categoryId = categoryService.GetAll().GetRandomElement().CategoryId;
            var supplierId = supplierService.GetAll().GetRandomElement().SupplierId;
            var product = new ProductDb
            {
                UnitPrice = 10,
                UnitsInStock = 100,
                CategoryId = categoryId,
                SupplierId = supplierId,
            };

            var createdProduct = productService.Create(product);
            
            createdProduct.UnitPrice = 15;
            createdProduct.UnitsInStock = 50;

            var updatedProduct = productService.Update(createdProduct);

            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(15, updatedProduct.UnitPrice);
            Assert.AreEqual(50, updatedProduct.UnitsInStock);
        }

        [TestMethod]
        public void ShouldDeleteProduct()
        {
            var categoryId = categoryService.GetAll().GetRandomElement().CategoryId;
            var supplierId = supplierService.GetAll().GetRandomElement().SupplierId;
            var product = new ProductDb
            {
                UnitPrice = 10,
                UnitsInStock = 100,
                CategoryId = categoryId,
                SupplierId = supplierId,
            };

            var createdProduct = productService.Create(product);


            productService.Delete(createdProduct.ProductId);
            var deletedProduct = productService.GetById(createdProduct.ProductId);

            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        public void ShouldGetAllProducts()
        {
            var result = productService.GetAll();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldGetProductById()
        {
            var categoryId = categoryService.GetAll().GetRandomElement().CategoryId;
            var supplierId = supplierService.GetAll().GetRandomElement().SupplierId;
            var product = new ProductDb
            {
                UnitPrice = 10,
                UnitsInStock = 100,
                CategoryId = categoryId,
                SupplierId = supplierId,
            };

            var createdProduct = productService.Create(product);
            var result = productService.GetById(createdProduct.ProductId);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.UnitPrice);
            Assert.AreEqual(100, result.UnitsInStock);
        }
    }
}
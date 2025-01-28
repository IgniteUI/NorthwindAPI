using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class ProductServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateProduct()
        {
            var product = DataHelper.GetProduct();
            var createdProduct = await DataHelper.CreateProduct(product);

            Assert.IsNotNull(createdProduct);
            createdProduct = DataHelper2.ProductService.GetById(createdProduct.ProductId);
            Assert.IsNotNull(createdProduct);
            Assert.AreEqual(product.UnitPrice, createdProduct.UnitPrice);
            Assert.AreEqual(product.UnitsInStock, createdProduct.UnitsInStock);
        }

        [TestMethod]
        public async Task ShouldUpdateProduct()
        {
            var createdProduct = await DataHelper.CreateProduct();
            double? originaUnitPrice = createdProduct.UnitPrice;
            double? originaUnitsInStock = createdProduct.UnitsInStock;
            createdProduct.UnitPrice = 15;
            createdProduct.UnitsInStock = 50;

            var updatedProduct = await DataHelper.ProductService.Update(createdProduct, createdProduct.ProductId);

            Assert.IsNotNull(updatedProduct);
            updatedProduct = DataHelper2.ProductService.GetById(updatedProduct.ProductId);
            Assert.IsNotNull(updatedProduct);
            Assert.AreNotEqual(originaUnitPrice, updatedProduct.UnitPrice);
            Assert.AreNotEqual(originaUnitsInStock, updatedProduct.UnitsInStock);

            Assert.AreEqual(createdProduct.UnitPrice, updatedProduct.UnitPrice);
            Assert.AreEqual(createdProduct.UnitsInStock, updatedProduct.UnitsInStock);
        }

        [TestMethod]
        public async Task ShouldDeleteProduct()
        {
            var createdProduct = await DataHelper.CreateProduct();

            DataHelper.ProductService.Delete(createdProduct.ProductId);
            var deletedProduct = DataHelper2.ProductService.GetById(createdProduct.ProductId);

            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        public async Task ShouldGetAllProducts()
        {
            await DataHelper.CreateProduct();
            await DataHelper.CreateProduct();

            var result = DataHelper2.ProductService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public async Task ShouldGetProductById()
        {
            var createdProduct = await DataHelper.CreateProduct();
            var result = DataHelper2.ProductService.GetById(createdProduct.ProductId);

            Assert.IsNotNull(result);
            Assert.AreEqual(createdProduct.UnitPrice, result.UnitPrice);
            Assert.AreEqual(createdProduct.UnitsInStock, result.UnitsInStock);
        }
    }
}
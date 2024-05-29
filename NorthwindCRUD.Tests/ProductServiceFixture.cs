using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class ProductServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateProduct()
        {
            var product = DataHelper.GetProduct();
            var createdProduct = DataHelper.CreateProduct(product);

            Assert.IsNotNull(createdProduct);
            createdProduct = DataHelper2.ProductService.GetById(createdProduct.ProductId);
            Assert.IsNotNull(createdProduct);
            Assert.AreEqual(product.UnitPrice, createdProduct.UnitPrice);
            Assert.AreEqual(product.UnitsInStock, createdProduct.UnitsInStock);
        }

        [TestMethod]
        public void ShouldUpdateProduct()
        {
            var createdProduct = DataHelper.CreateProduct();
            double? originaUnitPrice = createdProduct.UnitPrice;
            double? originaUnitsInStock = createdProduct.UnitsInStock;
            createdProduct.UnitPrice = 15;
            createdProduct.UnitsInStock = 50;

            var updatedProduct = DataHelper.ProductService.Update(createdProduct);

            Assert.IsNotNull(updatedProduct);
            updatedProduct = DataHelper2.ProductService.GetById(updatedProduct.ProductId);
            Assert.IsNotNull(updatedProduct);
            Assert.AreNotEqual(originaUnitPrice, updatedProduct.UnitPrice);
            Assert.AreNotEqual(originaUnitsInStock, updatedProduct.UnitsInStock);

            Assert.AreEqual(createdProduct.UnitPrice, updatedProduct.UnitPrice);
            Assert.AreEqual(createdProduct.UnitsInStock, updatedProduct.UnitsInStock);
        }

        [TestMethod]
        public void ShouldDeleteProduct()
        {
            var createdProduct = DataHelper.CreateProduct();

            DataHelper.ProductService.Delete(createdProduct.ProductId);
            var deletedProduct = DataHelper2.ProductService.GetById(createdProduct.ProductId);

            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        public void ShouldGetAllProducts()
        {
            DataHelper.CreateProduct();
            DataHelper.CreateProduct();

            var result = DataHelper2.ProductService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetProductById()
        {
            var createdProduct = DataHelper.CreateProduct();
            var result = DataHelper2.ProductService.GetById(createdProduct.ProductId);

            Assert.IsNotNull(result);
            Assert.AreEqual(createdProduct.UnitPrice, result.UnitPrice);
            Assert.AreEqual(createdProduct.UnitsInStock, result.UnitsInStock);
        }
    }
}
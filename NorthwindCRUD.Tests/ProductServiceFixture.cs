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
            Assert.AreEqual(product.UnitPrice, createdProduct.UnitPrice);
            Assert.AreEqual(product.UnitsInStock, createdProduct.UnitsInStock);
        }

        [TestMethod]
        public void ShouldUpdateProduct()
        {
            var product = DataHelper.GetProduct();
            var createdProduct = DataHelper.CreateProduct(product);

            createdProduct.UnitPrice = 15;
            createdProduct.UnitsInStock = 50;

            var updatedProduct = DataHelper.ProductService.Update(createdProduct);

            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(15, updatedProduct.UnitPrice);
            Assert.AreEqual(50, updatedProduct.UnitsInStock);
        }

        [TestMethod]
        public void ShouldDeleteProduct()
        {
            var product = DataHelper.GetProduct();
            var createdProduct = DataHelper.CreateProduct(product);

            DataHelper.ProductService.Delete(createdProduct.ProductId);
            var deletedProduct = DataHelper.ProductService.GetById(createdProduct.ProductId);

            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        public void ShouldGetAllProducts()
        {
            DataHelper.CreateProduct(DataHelper.GetProduct());
            DataHelper.CreateProduct(DataHelper.GetProduct());

            var result = DataHelper.ProductService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetProductById()
        {
            var product = DataHelper.GetProduct();
            var createdProduct = DataHelper.CreateProduct(product);
            var result = DataHelper.ProductService.GetById(createdProduct.ProductId);

            Assert.IsNotNull(result);
            Assert.AreEqual(product.UnitPrice, result.UnitPrice);
            Assert.AreEqual(product.UnitsInStock, result.UnitsInStock);
        }
    }
}
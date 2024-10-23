using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class SalesServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldRetrieveSalesDataByYear()
        {
            var order1 = await DataHelper.CreateOrder("2023-10-10T00:00:00");
            var order2 = await DataHelper.CreateOrder("2023-01-01T00:00:00");

            SalesDto[] salesData = DataHelper2.SalesService.RetrieveSalesDataByYear(2023, 0, 0);

            Assert.IsNotNull(salesData);
            Assert.AreEqual(2, salesData.Length);

            var orderDetail1 = DataHelper2.OrderService.GetOrderDetailsById(order1.OrderId);
            var orderDetail2 = DataHelper2.OrderService.GetOrderDetailsById(order2.OrderId);
            Assert.AreEqual(orderDetail1.Sum(od => od.Quantity) + orderDetail2.Sum(od => od.Quantity), salesData.Sum(s => s.QuantitySold));
            Assert.AreEqual(
                            orderDetail1.Sum(od => od.Quantity * od.UnitPrice) +
                            orderDetail2.Sum(od => od.Quantity * od.UnitPrice),
                            salesData.Sum(s => s.SaleAmount));
        }

        [TestMethod]
        public async Task ShouldRetrieveSalesDataByCountry()
        {
            var order2 = await DataHelper.CreateOrder("2023-01-01T00:00:00", "USA");
            var order1 = await DataHelper.CreateOrder("2023-10-10T00:00:00", "USA");

            SalesDto[] salesData = DataHelper.SalesService.RetrieveSalesDataByCountry("2023-01-01", "2023-12-31", "USA");

            Assert.IsNotNull(salesData);
            Assert.AreEqual(2, salesData.Length);

            var orderDetail1 = DataHelper.OrderService.GetOrderDetailsById(order1.OrderId);
            var orderDetail2 = DataHelper.OrderService.GetOrderDetailsById(order2.OrderId);
            Assert.AreEqual(orderDetail1.Sum(od => od.Quantity) + orderDetail2.Sum(od => od.Quantity), salesData.Sum(s => s.QuantitySold));
            Assert.AreEqual(
                            orderDetail1.Sum(od => od.Quantity * od.UnitPrice) +
                            orderDetail2.Sum(od => od.Quantity * od.UnitPrice),
                            salesData.Sum(s => s.SaleAmount));

            // Check that other country doesn't have any sales.
            salesData = DataHelper.SalesService.RetrieveSalesDataByCountry("2023-01-01", "2023-12-31", "Canada");
            Assert.AreEqual(0, salesData.Length);
        }

        [TestMethod]
        public async Task ShouldRetrieveSalesDataByProductCategoryAndYear()
        {
            var product = await DataHelper.CreateProduct();
            var order2 = await DataHelper.CreateOrder("2023-01-01T00:00:00", product: product, quantity: 10);
            var order1 = await DataHelper.CreateOrder("2023-10-10T00:00:00", product: product, quantity: 20);

            Assert.IsNotNull(product.CategoryId);

            var category = DataHelper2.CategoryService.GetById((int)product.CategoryId);
            SalesDto[] salesData = DataHelper2.SalesService.GetSalesDataByCategoryAndYear(category.Name, 2023);

            Assert.IsNotNull(salesData);
            Assert.AreEqual(2, salesData.Length);

            var orderDetail1 = DataHelper2.OrderService.GetOrderDetailsById(order1.OrderId);
            var orderDetail2 = DataHelper2.OrderService.GetOrderDetailsById(order2.OrderId);
            Assert.AreEqual(orderDetail1.Sum(od => od.Quantity) + orderDetail2.Sum(od => od.Quantity), salesData.Sum(s => s.QuantitySold));
            Assert.AreEqual(
                            orderDetail1.Sum(od => od.Quantity * od.UnitPrice) +
                            orderDetail2.Sum(od => od.Quantity * od.UnitPrice),
                            salesData.Sum(s => s.SaleAmount));
        }
    }
}
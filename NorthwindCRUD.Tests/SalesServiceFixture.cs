using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class SalesServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldRetrieveSalesDataByYear()
        {
            var order1 = DataHelper.CreateOrder("2023-10-10T00:00:00");
            var order2 = DataHelper.CreateOrder("2023-01-01T00:00:00");

            SalesDto[] salesData = DataHelper.SalesService.RetrieveSalesDataByYear(2023, 0, 0);

            Assert.IsNotNull(salesData);
            Assert.AreEqual(2, salesData.Length);

            var orderDetail1 = DataHelper.OrderService.GetOrderDetailsById(order1.OrderId);
            var orderDetail2 = DataHelper.OrderService.GetOrderDetailsById(order2.OrderId);
            Assert.AreEqual(orderDetail1.Sum(od => od.Quantity) + orderDetail2.Sum(od => od.Quantity), salesData.Sum(s => s.QuantitySold));
            Assert.AreEqual(
                            orderDetail1.Sum(od => od.Quantity * od.UnitPrice) +
                            orderDetail2.Sum(od => od.Quantity * od.UnitPrice),
                            salesData.Sum(s => s.SaleAmount));
        }
    }
}
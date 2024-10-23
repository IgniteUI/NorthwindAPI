using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class OrderServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateOrder()
        {
            var order = DataHelper.GetOrder();
            var customer = await DataHelper.CreateCustomer();
            var employee = await DataHelper.CreateEmployee();
            var shipper = await DataHelper.CreateShipper();
            order.CustomerId = customer.CustomerId;
            order.EmployeeId = employee.EmployeeId;
            order.ShipperId = shipper.ShipperId;

            var createdOrder = await DataHelper.OrderService.Create(order);

            Assert.IsNotNull(createdOrder);
            createdOrder = DataHelper2.OrderService.GetById(createdOrder.OrderId);
            Assert.IsNotNull(createdOrder);

            Assert.AreEqual(order.CustomerId, createdOrder.CustomerId);
            Assert.AreEqual(order.EmployeeId, createdOrder.EmployeeId);
            Assert.AreEqual(order.ShipperId, createdOrder.ShipperId);
            Assert.AreEqual(order.OrderDate, createdOrder.OrderDate);
            Assert.AreEqual(order.RequiredDate, createdOrder.RequiredDate);
            Assert.AreEqual(order.ShipVia, createdOrder.ShipVia);
            Assert.AreEqual(order.Freight, createdOrder.Freight);
            Assert.AreEqual(order.ShipName, createdOrder.ShipName);
            Assert.AreEqual(order.ShipAddress.City, createdOrder.ShipAddress.City);
        }

        [TestMethod]
        public async Task ShouldUpdateOrder()
        {
            var order = await DataHelper.CreateOrder();
            string? originalCustomerId = order.CustomerId;
            int? originalEmployeeId = order.EmployeeId;

            order.CustomerId = (await DataHelper.CreateCustomer()).CustomerId;
            order.EmployeeId = (await DataHelper.CreateEmployee()).EmployeeId;

            var updatedOrder = await DataHelper.OrderService.Update(order, order.OrderId);

            Assert.IsNotNull(updatedOrder);
            updatedOrder = DataHelper2.OrderService.GetById(updatedOrder.OrderId);
            Assert.IsNotNull(updatedOrder);
            Assert.AreNotEqual(originalCustomerId, updatedOrder.CustomerId);
            Assert.AreNotEqual(originalEmployeeId, updatedOrder.EmployeeId);
            Assert.AreEqual(order.CustomerId, updatedOrder.CustomerId);
            Assert.AreEqual(order.EmployeeId, updatedOrder.EmployeeId);
        }

        [TestMethod]
        public async Task ShouldDeleteOrder()
        {
            var order = await DataHelper.CreateOrder();

            DataHelper.OrderService.Delete(order.OrderId);
            var deletedOrder = DataHelper2.OrderService.GetById(order.OrderId);

            Assert.IsNull(deletedOrder);
        }

        [TestMethod]
        public async Task ShouldGetAllOrders()
        {
            await DataHelper.CreateOrder();
            await DataHelper.CreateOrder();

            var result = DataHelper2.OrderService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public async Task ShouldGetOrderById()
        {
            var order = await DataHelper.CreateOrder();

            var result = DataHelper2.OrderService.GetById(order.OrderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(order.OrderId, result.OrderId);
            Assert.AreEqual(order.CustomerId, result.CustomerId);
            Assert.AreEqual(order.EmployeeId, result.EmployeeId);
        }
    }
}
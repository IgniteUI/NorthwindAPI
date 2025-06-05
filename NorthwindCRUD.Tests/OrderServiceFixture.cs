using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class OrderServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateOrder()
        {
            var order = DataHelper.GetOrder();
            var customer = DataHelper.CreateCustomer();
            var employee = DataHelper.CreateEmployee();
            var shipper = DataHelper.CreateShipper();
            order.CustomerId = customer.CustomerId;
            order.EmployeeId = employee.EmployeeId;
            order.ShipperId = shipper.ShipperId;

            var createdOrder = DataHelper.OrderService.Create(order);

            Assert.IsNotNull(createdOrder);
            createdOrder = DataHelper2.OrderService.GetById(createdOrder.OrderId);
            Assert.IsNotNull(createdOrder);

            Assert.AreEqual(order.OrderId, createdOrder.OrderId);
            Assert.AreEqual(order.CustomerId, createdOrder.CustomerId);
            Assert.AreEqual(order.EmployeeId, createdOrder.EmployeeId);
        }

        [TestMethod]
        public void ShouldUpdateOrder()
        {
            var order = DataHelper.CreateOrder();
            string? originalCustomerId = order.CustomerId;
            int? originalEmployeeId = order.EmployeeId;

            order.CustomerId = DataHelper.CreateCustomer().CustomerId;
            order.EmployeeId = DataHelper.CreateEmployee().EmployeeId;

            var updatedOrder = DataHelper.OrderService.Update(order.OrderId, order);

            Assert.IsNotNull(updatedOrder);
            updatedOrder = DataHelper2.OrderService.GetById(updatedOrder.OrderId);
            Assert.IsNotNull(updatedOrder);
            Assert.AreNotEqual(originalCustomerId, updatedOrder.CustomerId);
            Assert.AreNotEqual(originalEmployeeId, updatedOrder.EmployeeId);
            Assert.AreEqual(order.CustomerId, updatedOrder.CustomerId);
            Assert.AreEqual(order.EmployeeId, updatedOrder.EmployeeId);
        }

        [TestMethod]
        public void ShouldDeleteOrder()
        {
            var order = DataHelper.CreateOrder();

            DataHelper.OrderService.Delete(order.OrderId);
            var deletedOrder = DataHelper2.OrderService.GetById(order.OrderId);

            Assert.IsNull(deletedOrder);
        }

        [TestMethod]
        public void ShouldGetAllOrders()
        {
            DataHelper.CreateOrder();
            DataHelper.CreateOrder();

            var result = DataHelper2.OrderService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldGetOrderById()
        {
            var order = DataHelper.CreateOrder();

            var result = DataHelper2.OrderService.GetById(order.OrderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(order.OrderId, result.OrderId);
            Assert.AreEqual(order.CustomerId, result.CustomerId);
            Assert.AreEqual(order.EmployeeId, result.EmployeeId);
        }
    }
}
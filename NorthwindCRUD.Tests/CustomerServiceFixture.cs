using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class CustomerServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateCustomer()
        {
            var customer = DataHelper.GetCustomer();
            var createdCustomer = await DataHelper.CustomerService.Create(customer);
            Assert.IsNotNull(createdCustomer);
            Assert.AreEqual(customer.CompanyName, createdCustomer.CompanyName);
            Assert.AreEqual(customer.ContactName, createdCustomer.ContactName);
            Assert.AreEqual(customer.ContactTitle, createdCustomer.ContactTitle);
            Assert.AreEqual(customer.Address.Street, createdCustomer.Address.Street);
            Assert.AreEqual(customer.Address.City, createdCustomer.Address.City);
            Assert.AreEqual(customer.Address.PostalCode, createdCustomer.Address.PostalCode);
            Assert.AreEqual(customer.Address.Country, createdCustomer.Address.Country);
            Assert.AreEqual(customer.Address.Phone, createdCustomer.Address.Phone);

            createdCustomer = DataHelper2.CustomerService.GetById(createdCustomer.CustomerId);
            Assert.IsNotNull(createdCustomer);

            Assert.AreEqual(customer.CompanyName, createdCustomer.CompanyName);
            Assert.AreEqual(customer.ContactName, createdCustomer.ContactName);
            Assert.AreEqual(customer.ContactTitle, createdCustomer.ContactTitle);
            Assert.AreEqual(customer.Address.Street, createdCustomer.Address.Street);
            Assert.AreEqual(customer.Address.City, createdCustomer.Address.City);
            Assert.AreEqual(customer.Address.PostalCode, createdCustomer.Address.PostalCode);
            Assert.AreEqual(customer.Address.Country, createdCustomer.Address.Country);
            Assert.AreEqual(customer.Address.Phone, createdCustomer.Address.Phone);
        }

        [TestMethod]
        public async Task ShouldReturnAllCustomers()
        {
            await DataHelper.CustomerService.Create(DataHelper.GetCustomer());
            await DataHelper.CustomerService.Create(DataHelper.GetCustomer());

            var result = DataHelper2.CustomerService.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }
    }
}
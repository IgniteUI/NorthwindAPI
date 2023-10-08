using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Services;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class CustomerServiceFixture : BaseFixture
    {
        private CustomerService customerService;

        [TestInitialize]
        public void Initialize()
        {
            DataContext context = GetInMemoryDatabaseContext();
            customerService = new CustomerService(context);
        }

        [TestMethod]
        public void ShouldCreateCustomer()
        {
            var address = new AddressDb
            {
                Street = "6955 Union Park Center Suite 500",
                City = "Midvale",
                PostalCode = "84047",
                Region = "",
                Country = "USA",
                Phone = "(800) 231-8588",
            };

            var customer = new CustomerDb
            {
                CustomerId = "12345",
                CompanyName = "Infragistics",
                ContactName = "Maria Anders",
                ContactTitle = "Sales Representative",
                Address = address,
            };

            var createdCustomer = customerService.Create(customer);
            
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
        public void ShouldUpdateEmployee()
        {
        }

        [TestMethod]
        public void ShouldDeleteEmployee()
        {
        }

        [TestMethod]
        public void ShouldReturnAllCustomers()
        {
            var result = customerService.GetAll();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldReturnEmployeesByReportsTo()
        {            
        }
    }
}
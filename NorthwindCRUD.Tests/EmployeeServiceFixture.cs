using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class EmployeeServiceFixture : BaseFixture
    {
        private EmployeeService employeeService = null!;

        [TestInitialize]
        public void Initialize()
        {
            DataContext context = GetInMemoryDatabaseContext();
            employeeService = new EmployeeService(context);
        }

        [TestMethod]
        public void ShouldCreateEmployee()
        {
            var employee = new EmployeeDb
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
            };

            var createdEmployee = employeeService.Create(employee);

            Assert.IsNotNull(createdEmployee);
            Assert.AreEqual(employee.FirstName, createdEmployee.FirstName);
            Assert.AreEqual(employee.LastName, createdEmployee.LastName);
            Assert.AreEqual(employee.Title, createdEmployee.Title);
        }

        [TestMethod]
        public void ShouldUpdateEmployee()
        {
            var employee = new EmployeeDb
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
            };
            var createdEmployee = employeeService.Create(employee);

            createdEmployee.Title = "Director";
            var updatedEmployee = employeeService.Update(createdEmployee);

            Assert.IsNotNull(updatedEmployee);
            Assert.AreEqual("Director", updatedEmployee.Title);
        }

        [TestMethod]
        public void ShouldDeleteEmployee()
        {
            var employee = new EmployeeDb
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
            };
            var createdEmployee = employeeService.Create(employee);

            employeeService.Delete(createdEmployee.EmployeeId);
            var deletedEmployee = employeeService.GetById(createdEmployee.EmployeeId);

            Assert.IsNull(deletedEmployee);
        }

        [TestMethod]
        public void ShouldReturnAllEmployees()
        {
            var result = employeeService.GetAll();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void ShouldReturnEmployeesByReportsTo()
        {
            var manager = new EmployeeDb
            {
                FirstName = "Manager",
                LastName = "Doe",
                Title = "Manager",
            };

            var createdManager = employeeService.Create(manager);
            var employee1 = new EmployeeDb
            {
                FirstName = "Employee1",
                LastName = "Smith",
                Title = "Employee",
                ReportsTo = createdManager.EmployeeId,
            };

            var employee2 = new EmployeeDb
            {
                FirstName = "Employee2",
                LastName = "Johnson",
                Title = "Employee",
                ReportsTo = createdManager.EmployeeId,
            };

            var createdEmployee1 = employeeService.Create(employee1);
            var createdEmployee2 = employeeService.Create(employee2);

            var result = employeeService.GetEmployeesByReportsTo(createdManager.EmployeeId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.All(e => e.ReportsTo == createdManager.EmployeeId));
        }

        [TestMethod]
        public void ShouldReturnEmployeeById()
        {
            var employee = new EmployeeDb
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
            };
            var createdEmployee = employeeService.Create(employee);

            var result = employeeService.GetById(createdEmployee.EmployeeId);

            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
            Assert.AreEqual("Manager", result.Title);
        }
    }
}
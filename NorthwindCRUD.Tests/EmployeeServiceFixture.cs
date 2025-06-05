using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class EmployeeServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateEmployee()
        {
            EmployeeDb employee = DataHelper.GetEmployee();

            var createdEmployee = DataHelper.EmployeeService.Create(employee);

            Assert.IsNotNull(createdEmployee);
            createdEmployee = DataHelper2.EmployeeService.GetById(createdEmployee.EmployeeId);
            Assert.IsNotNull(createdEmployee);
            Assert.AreEqual(employee.FirstName, createdEmployee.FirstName);
            Assert.AreEqual(employee.LastName, createdEmployee.LastName);
            Assert.AreEqual(employee.Title, createdEmployee.Title);
        }

        [TestMethod]
        public void ShouldUpdateEmployee()
        {
            var employee = DataHelper.GetEmployee();
            string originalTitle = employee.Title;
            var createdEmployee = DataHelper.EmployeeService.Create(employee);

            createdEmployee.Title = "Director";
            var updatedEmployee = DataHelper.EmployeeService.Update(createdEmployee.EmployeeId, createdEmployee);

            Assert.IsNotNull(updatedEmployee);
            updatedEmployee = DataHelper2.EmployeeService.GetById(updatedEmployee.EmployeeId);
            Assert.IsNotNull(updatedEmployee);
            Assert.AreNotEqual(originalTitle, updatedEmployee.Title);
            Assert.AreEqual(createdEmployee.Title, updatedEmployee.Title);
        }

        [TestMethod]
        public void ShouldDeleteEmployee()
        {
            var employee = DataHelper.GetEmployee();
            var createdEmployee = DataHelper.EmployeeService.Create(employee);

            DataHelper.EmployeeService.Delete(createdEmployee.EmployeeId);
            var deletedEmployee = DataHelper2.EmployeeService.GetById(createdEmployee.EmployeeId);

            Assert.IsNull(deletedEmployee);
        }

        [TestMethod]
        public void ShouldReturnAllEmployees()
        {
            DataHelper.EmployeeService.Create(DataHelper.GetEmployee());
            DataHelper.EmployeeService.Create(DataHelper.GetEmployee());

            var result = DataHelper2.EmployeeService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void ShouldReturnEmployeesByReportsTo()
        {
            var manager = DataHelper.GetEmployee();

            var createdManager = DataHelper.EmployeeService.Create(manager);
            var employee1 = DataHelper.GetEmployee();
            employee1.ReportsTo = createdManager.EmployeeId;

            var employee2 = DataHelper.GetEmployee();
            employee2.ReportsTo = createdManager.EmployeeId;

            var createdEmployee1 = DataHelper.EmployeeService.Create(employee1);
            var createdEmployee2 = DataHelper.EmployeeService.Create(employee2);

            var result = DataHelper.EmployeeService.GetEmployeesByReportsTo(createdManager.EmployeeId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.All(e => e.ReportsTo == createdManager.EmployeeId));
        }

        [TestMethod]
        public void ShouldReturnEmployeeById()
        {
            var employee = DataHelper.GetEmployee();

            var createdEmployee = DataHelper.EmployeeService.Create(employee);

            var result = DataHelper.EmployeeService.GetById(createdEmployee.EmployeeId);

            Assert.IsNotNull(result);
            Assert.AreEqual(employee.FirstName, result.FirstName);
            Assert.AreEqual(employee.LastName, result.LastName);
            Assert.AreEqual(employee.Title, result.Title);
        }
    }
}
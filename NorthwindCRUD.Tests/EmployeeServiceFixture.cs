using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class EmployeeServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateEmployee()
        {
            var employee = DataHelper.GetEmployee();

            var createdEmployee = await DataHelper.EmployeeService.Create(employee);

            Assert.IsNotNull(createdEmployee);
            createdEmployee = DataHelper2.EmployeeService.GetById(createdEmployee.EmployeeId);
            Assert.IsNotNull(createdEmployee);
            Assert.AreEqual(employee.FirstName, createdEmployee.FirstName);
            Assert.AreEqual(employee.LastName, createdEmployee.LastName);
            Assert.AreEqual(employee.Title, createdEmployee.Title);
        }

        [TestMethod]
        public async Task ShouldUpdateEmployee()
        {
            var employee = DataHelper.GetEmployee();
            string originalTitle = employee.Title;
            var createdEmployee = await DataHelper.EmployeeService.Create(employee);

            createdEmployee.Title = "Director";
            var updatedEmployee = await DataHelper.EmployeeService.Update(createdEmployee, createdEmployee.EmployeeId);

            Assert.IsNotNull(updatedEmployee);
            updatedEmployee = DataHelper2.EmployeeService.GetById(updatedEmployee.EmployeeId);
            Assert.IsNotNull(updatedEmployee);
            Assert.AreNotEqual(originalTitle, updatedEmployee.Title);
            Assert.AreEqual(createdEmployee.Title, updatedEmployee.Title);
        }

        [TestMethod]
        public async Task ShouldDeleteEmployee()
        {
            var employee = DataHelper.GetEmployee();
            var createdEmployee = await DataHelper.EmployeeService.Create(employee);

            DataHelper.EmployeeService.Delete(createdEmployee.EmployeeId);
            var deletedEmployee = DataHelper2.EmployeeService.GetById(createdEmployee.EmployeeId);

            Assert.IsNull(deletedEmployee);
        }

        [TestMethod]
        public async Task ShouldReturnAllEmployees()
        {
            await DataHelper.EmployeeService.Create(DataHelper.GetEmployee());
            await DataHelper.EmployeeService.Create(DataHelper.GetEmployee());

            var result = DataHelper2.EmployeeService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public async Task ShouldReturnEmployeesByReportsTo()
        {
            var manager = DataHelper.GetEmployee();

            var createdManager = await DataHelper.EmployeeService.Create(manager);
            var employee1 = DataHelper.GetEmployee();
            employee1.ReportsTo = createdManager.EmployeeId;

            var employee2 = DataHelper.GetEmployee();
            employee2.ReportsTo = createdManager.EmployeeId;

            var createdEmployee1 = await DataHelper.EmployeeService.Create(employee1);
            var createdEmployee2 = await DataHelper.EmployeeService.Create(employee2);

            var result = DataHelper.EmployeeService.GetEmployeesByReportsTo(createdManager.EmployeeId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.All(e => e.ReportsTo == createdManager.EmployeeId));
        }

        [TestMethod]
        public async Task ShouldReturnEmployeeById()
        {
            var employee = DataHelper.GetEmployee();

            var createdEmployee = await DataHelper.EmployeeService.Create(employee);

            var result = DataHelper.EmployeeService.GetById(createdEmployee.EmployeeId);

            Assert.IsNotNull(result);
            Assert.AreEqual(employee.FirstName, result.FirstName);
            Assert.AreEqual(employee.LastName, result.LastName);
            Assert.AreEqual(employee.Title, result.Title);
        }
    }
}
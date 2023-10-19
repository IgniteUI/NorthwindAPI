using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class EmployeeTerritoryServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldAddTerritoryToEmployee()
        {
            var employeeId = DataHelper.CreateEmployee().EmployeeId;
            var territoryId = DataHelper.CreateTerritory().TerritoryId;

            DataHelper.CreateEmployeeTerritory(employeeId, territoryId);

            var territories = DataHelper2.EmployeeTerritoryService.GetTeritoriesByEmployeeId(employeeId);
            Assert.IsNotNull(territories);
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId));
        }

        [TestMethod]
        public void ShouldReturnTerritoriesForEmployee()
        {
            var employeeId = DataHelper.CreateEmployee().EmployeeId;
            var territoryId1 = DataHelper.CreateTerritory().TerritoryId;
            var territoryId2 = DataHelper.CreateTerritory().TerritoryId;

            DataHelper.CreateEmployeeTerritory(employeeId, territoryId1);
            DataHelper.CreateEmployeeTerritory(employeeId, territoryId2);

            var territories = DataHelper2.EmployeeTerritoryService.GetTeritoriesByEmployeeId(employeeId);

            Assert.IsNotNull(territories);
            Assert.AreEqual(2, territories.Length);
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId1));
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId2));
        }

        [TestMethod]
        public void ShouldReturnEmployeesForTerritory()
        {
            var territoryId = DataHelper.CreateTerritory().TerritoryId;
            var employeeId1 = DataHelper.CreateEmployee().EmployeeId;
            var employeeId2 = DataHelper.CreateEmployee().EmployeeId;

            DataHelper.CreateEmployeeTerritory(employeeId1, territoryId);
            DataHelper.CreateEmployeeTerritory(employeeId2, territoryId);

            var employees = DataHelper2.EmployeeTerritoryService.GetEmployeesByTerritoryId(territoryId);

            Assert.IsNotNull(employees);
            Assert.AreEqual(2, employees.Length);
            Assert.IsTrue(employees.Any(e => e.EmployeeId == employeeId1));
            Assert.IsTrue(employees.Any(e => e.EmployeeId == employeeId2));
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class EmployeeTerritoryServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldAddTerritoryToEmployee()
        {
            var employeeId = (await DataHelper.CreateEmployee()).EmployeeId;
            var territoryId = (await DataHelper.CreateTerritory()).TerritoryId;

            DataHelper.CreateEmployeeTerritory(employeeId, territoryId);

            var territories = DataHelper2.EmployeeTerritoryService.GetTeritoriesByEmployeeId(employeeId);
            Assert.IsNotNull(territories);
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId));
        }

        [TestMethod]
        public async Task ShouldReturnTerritoriesForEmployee()
        {
            var employeeId = (await DataHelper.CreateEmployee()).EmployeeId;
            var territoryId1 = (await DataHelper.CreateTerritory()).TerritoryId;
            var territoryId2 = (await DataHelper.CreateTerritory()).TerritoryId;

            DataHelper.CreateEmployeeTerritory(employeeId, territoryId1);
            DataHelper.CreateEmployeeTerritory(employeeId, territoryId2);

            var territories = DataHelper2.EmployeeTerritoryService.GetTeritoriesByEmployeeId(employeeId);

            Assert.IsNotNull(territories);
            Assert.AreEqual(2, territories.Length);
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId1));
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId2));
        }

        [TestMethod]
        public async Task ShouldReturnEmployeesForTerritory()
        {
            var territoryId = (await DataHelper.CreateTerritory()).TerritoryId;
            var employeeId1 = (await DataHelper.CreateEmployee()).EmployeeId;
            var employeeId2 = (await DataHelper.CreateEmployee()).EmployeeId;

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
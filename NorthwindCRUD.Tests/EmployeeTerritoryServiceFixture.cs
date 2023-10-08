using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Services;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class EmployeeTerritoryServiceFixture : BaseFixture
    {
        private EmployeeTerritoryService employeeTerritoryService;
        private EmployeeService employeeService;
        private TerritoryService territoryService;

        [TestInitialize]
        public void Initialize()
        {
            DataContext context = GetInMemoryDatabaseContext();
            employeeService = new EmployeeService(context);
            territoryService = new TerritoryService(context);
            employeeTerritoryService = new EmployeeTerritoryService(context);
        }

        [TestMethod]
        public void ShouldAddTerritoryToEmployee()
        {
            var employeeId = employeeService.GetAll().GetRandomElement().EmployeeId;
            var territoryId = territoryService.GetAll().GetRandomElement().TerritoryId;

            var employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = employeeId,
                TerritoryId = territoryId,
            };

            employeeTerritoryService.AddTerritoryToEmployee(employeeTerritory);

            var territories = employeeTerritoryService.GetTeritoriesByEmployeeId(employeeId);
            Assert.IsNotNull(territories);
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId));
        }

        [TestMethod]
        public void ShouldReturnTerritoriesForEmployee()
        {
            var employeeId = employeeService.GetAll().GetRandomElement().EmployeeId;
            var territoryId1 = territoryService.GetAll().GetRandomElement().TerritoryId;
            var territoryId2 = territoryService.GetAll().GetRandomElement().TerritoryId;
            
            var initialTerritoryCount = employeeTerritoryService.GetTeritoriesByEmployeeId(employeeId).Count();

            var employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = employeeId,
                TerritoryId = territoryId1,
            };

            employeeTerritoryService.AddTerritoryToEmployee(employeeTerritory);
            
            employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = employeeId,
                TerritoryId = territoryId2,
            };

            employeeTerritoryService.AddTerritoryToEmployee(employeeTerritory);
            
            var territories = employeeTerritoryService.GetTeritoriesByEmployeeId(employeeId);

            Assert.IsNotNull(territories);
            Assert.AreEqual(initialTerritoryCount + 2, territories.Count());
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId1));
            Assert.IsTrue(territories.Any(t => t.TerritoryId == territoryId2));
        }

        [TestMethod]
        public void ShouldReturnEmployeesForTerritory()
        {
            var employeeId1 = employeeService.GetAll().GetRandomElement().EmployeeId;
            var employeeId2= employeeService.GetAll().GetRandomElement().EmployeeId;
            var territoryId = territoryService.GetAll().GetRandomElement().TerritoryId;

            var initialEmployeeCount = employeeTerritoryService.GetEmployeesByTerritoryId(territoryId).Count();

            var employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = employeeId1,
                TerritoryId = territoryId,
            };

            employeeTerritoryService.AddTerritoryToEmployee(employeeTerritory);

            employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = employeeId2,
                TerritoryId = territoryId,
            };

            employeeTerritoryService.AddTerritoryToEmployee(employeeTerritory);

            var employees = employeeTerritoryService.GetEmployeesByTerritoryId(territoryId);

            Assert.IsNotNull(employees);
            Assert.AreEqual(initialEmployeeCount + 2, employees.Count());
            Assert.IsTrue(employees.Any(e => e.EmployeeId == employeeId1));
            Assert.IsTrue(employees.Any(e => e.EmployeeId == employeeId2));
        }
    }
}
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

        static T GetRandomElement<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("The array cannot be null or empty.");
            }

            Random random = new Random();
            int randomIndex = random.Next(array.Length);
            return array[randomIndex];
        }

        [TestMethod]
        public void ShouldAddTerritoryToEmployee()
        {
            var employeeId = GetRandomElement(employeeService.GetAll()).EmployeeId;
            var territoryId = GetRandomElement(territoryService.GetAll()).TerritoryId;

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
            var employeeId = GetRandomElement(employeeService.GetAll()).EmployeeId;
            var territoryId1 = GetRandomElement(territoryService.GetAll()).TerritoryId;
            var territoryId2 = GetRandomElement(territoryService.GetAll()).TerritoryId;
            
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
            var employeeId1 = GetRandomElement(employeeService.GetAll()).EmployeeId;
            var employeeId2= GetRandomElement(employeeService.GetAll()).EmployeeId;
            var territoryId = GetRandomElement(territoryService.GetAll()).TerritoryId;

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
namespace NorthwindCRUD.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Models.Contracts;
    using NorthwindCRUD.Models.DbModels;


    public class EmployeeTerritoryService
    {
        private readonly IMapper mapper;
        private readonly DataContext dataContext;

        public EmployeeTerritoryService(IMapper mapper, DataContext dataContext)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        public EmployeeDb[] GetEmployeesByTerritoryId(string id)
        {
            var territory = this.dataContext.Territories
                .Include(t => t.EmployeesTerritories)
                .ThenInclude(t => t.Employee)
                .FirstOrDefault(t => t.TerritoryId == id);

            if (territory != null)
            {
                return territory.EmployeesTerritories.Select(et => et.Employee).ToArray();
            }

            return null;
        }

        
        public TerritoryDb[] GetTeritoriesByEmployeeId(int id)
        {

            var employee = this.dataContext.Employees
                .Include(c => c.Address)
                .Include(c => c.EmployeesTerritories)
                .ThenInclude(t => t.Territory)
                .FirstOrDefault(c => c.EmployeeId == id);

            if (employee != null)
            {
                return employee.EmployeesTerritories.Select(et => et.Territory).ToArray();
            }
            return null;
        }

        public EmployeeTerritoryDb AddTerritoryToEmployee(EmployeeTerritoryDb model)
        {
            var employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = model.EmployeeId,
                TerritoryId = model.TerritoryId
            };

            dataContext.EmployeesTerritories.Add(employeeTerritory);
            dataContext.SaveChanges();

            return employeeTerritory;
        }
    }
}

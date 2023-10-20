using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Constants;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class EmployeeTerritoryService
    {
        private readonly DataContext dataContext;

        public EmployeeTerritoryService(DataContext dataContext)
        {
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
            if (this.dataContext.Employees.FirstOrDefault(e => e.EmployeeId == model.EmployeeId) == null)
            {
                throw new InvalidOperationException(string.Format(StringTemplates.InvalidEntityMessage, nameof(model.Employee), model.EmployeeId.ToString()));
            }

            if (this.dataContext.Territories.FirstOrDefault(t => t.TerritoryId == model.TerritoryId) == null)
            {
                throw new InvalidOperationException(string.Format(StringTemplates.InvalidEntityMessage, nameof(model.Territory), model.TerritoryId.ToString()));
            }

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

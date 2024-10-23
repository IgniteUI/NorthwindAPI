using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Constants;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class EmployeeTerritoryService
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public EmployeeTerritoryService(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "Need to return nullable type")]
        public EmployeeDto[]? GetEmployeesByTerritoryId(string id)
        {
            var territory = this.dataContext.Territories
                .Include(t => t.EmployeesTerritories)
                .ThenInclude(t => t.Employee)
                .FirstOrDefault(t => t.TerritoryId == id);

            if (territory != null)
            {
                var employees = territory.EmployeesTerritories.Select(et => et.Employee).ToArray();
                return mapper.Map<EmployeeDto[]>(employees);
            }

            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "Need to return nullable type")]
        public TerritoryDto[]? GetTeritoriesByEmployeeId(int id)
        {
            var employee = this.dataContext.Employees
                .Include(c => c.Address)
                .Include(c => c.EmployeesTerritories)
                .ThenInclude(t => t.Territory)
                .FirstOrDefault(c => c.EmployeeId == id);

            if (employee != null)
            {
                var territories = employee.EmployeesTerritories.Select(et => et.Territory).ToArray();
                return mapper.Map<TerritoryDto[]>(territories);
            }

            return null;
        }

        public EmployeeTerritoryDto AddTerritoryToEmployee(EmployeeTerritoryDto model)
        {
            if (this.dataContext.Employees.FirstOrDefault(e => e.EmployeeId == model.EmployeeId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.EmployeeId), model.EmployeeId.ToString(CultureInfo.InvariantCulture)));
            }

            if (this.dataContext.Territories.FirstOrDefault(t => t.TerritoryId == model.TerritoryId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.TerritoryId), model.TerritoryId.ToString()));
            }

            var employeeTerritory = new EmployeeTerritoryDb
            {
                EmployeeId = model.EmployeeId,
                TerritoryId = model.TerritoryId,
            };

            dataContext.EmployeesTerritories.Add(employeeTerritory);
            dataContext.SaveChanges();

            return mapper.Map<EmployeeTerritoryDto>(employeeTerritory);
        }
    }
}

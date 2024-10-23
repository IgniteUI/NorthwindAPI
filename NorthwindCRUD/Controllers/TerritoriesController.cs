namespace NorthwindCRUD.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class TerritoriesController : BaseNorthwindAPIController<TerritoryDto, TerritoryDb, string>
    {
        private readonly RegionService regionService;
        private readonly EmployeeTerritoryService employeeTerritoryService;

        public TerritoriesController(TerritoryService territoryService, RegionService regionService, EmployeeTerritoryService employeeTerritoryService)
            : base(territoryService)
        {
            this.regionService = regionService;
            this.employeeTerritoryService = employeeTerritoryService;
        }

        [HttpGet("{id}/Employees")]
        public ActionResult<EmployeeDto[]> GetEmployeesByTerritory(string id)
        {
            var employees = this.employeeTerritoryService.GetEmployeesByTerritoryId(id);
            if (employees == null)
            {
                return NotFound($"No employees for territory {id}");
            }

            return Ok(employees);
        }

        [HttpGet("{id}/Region")]
        public ActionResult<RegionDto[]> GetRegionByTerritory(string id)
        {
            var territory = this.baseDbService.GetById(id);
            if (territory != null)
            {
                var region = this.regionService.GetById(territory.RegionId ?? default);

                if (region != null)
                {
                    return Ok(region);
                }
            }

            return NotFound();
        }
    }
}
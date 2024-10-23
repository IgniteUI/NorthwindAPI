namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class RegionsController : BaseNorthwindAPIController<RegionDto, RegionDb, int>
    {
        private readonly TerritoryService territoryService;

        public RegionsController(RegionService regionService, TerritoryService territoryService, OrderService ordersService)
            : base(regionService)
        {
            this.territoryService = territoryService;
        }

        [HttpGet("{id}/Territories")]
        public ActionResult<CustomerDto> GetTerritoryByRegionId(int id)
        {

            var region = this.baseDbService.GetById(id);
            if (region != null)
            {
                var territories = this.territoryService.GetTerritoriesByRegionId(id);
                return Ok(territories);
            }

            return NotFound();
        }
    }
}
namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly RegionService regionService;
        private readonly TerritoryService territoryService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public RegionsController(RegionService regionService, TerritoryService territoryService, IMapper mapper, ILogger logger)
        {
            this.regionService = regionService;
            this.territoryService = territoryService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<RegionDto[]> GetAll()
        {
            try
            {
                var regions = this.regionService.GetAll();
                return Ok(this.mapper.Map<RegionDb[], RegionDto[]>(regions));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }

        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<RegionDto> GetById(int id)
        {
            try
            {
                var region = this.regionService.GetById(id);
                if (region != null)
                {
                    return Ok(this.mapper.Map<RegionDb, RegionDto>(region));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Territory")]
        [Authorize]
        public ActionResult<CustomerDto> GetTerritoryByRegionId(int id)
        {
            try
            {
                var region = this.regionService.GetById(id);
                if (region != null)
                {
                    var territories = this.territoryService.GetTerritoriesByRegionId(id);
                    if (territories != null)
                    {
                        return Ok(this.mapper.Map<TerritoryDb[], TerritoryDto[]>(territories));
                    }
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<RegionDto> Create(RegionDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<RegionDto, RegionDb>(model);
                    var Region = this.regionService.Create(mappedModel);
                    return Ok(this.mapper.Map<RegionDb, RegionDto>(Region));
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<RegionDto> Update(RegionDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<RegionDto, RegionDb>(model);
                    var region = this.regionService.Update(mappedModel);

                    if (region != null)
                    {
                        return Ok(this.mapper.Map<RegionDb, RegionDto>(region));
                    }

                    return NotFound();
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<RegionDto> Delete(int id)
        {
            try
            {
                var region = this.regionService.Delete(id);
                if (region != null)
                {
                    return Ok(this.mapper.Map<RegionDb, RegionDto>(region));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
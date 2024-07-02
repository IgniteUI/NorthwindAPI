namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    [Route("[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly RegionService regionService;
        private readonly TerritoryService territoryService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(RegionService regionService, TerritoryService territoryService, PagingService pagingService, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionService = regionService;
            this.territoryService = territoryService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
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

        /// <summary>
        /// Fetches all regions or a page of regions based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the regions. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of regions to fetch. If this parameter is not provided, all regions are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the regions by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedRegions")]
        public ActionResult<PagedResultDto<RegionDto>> GetAllRegions(
            [FromQuery][Attributes.SwaggerSkipParameter] int? skip,
            [FromQuery][Attributes.SwaggerTopParameter] int? top,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve all regions
                var regions = this.regionService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<RegionDb, RegionDto>(regions, skip, top, null, null, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all regions or a page of regions based on the provided parameters.
        /// </summary>
        /// <param name="pageIndex">The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).</param>
        /// <param name="size">The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the records by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedRegionsWithPage")]
        public ActionResult<PagedResultDto<RegionDto>> GetPagedRegionsWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve regions as Queryable
                var regions = this.regionService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<RegionDb, RegionDto>(regions, null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
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

        [HttpGet("{id}/Territories")]
        public ActionResult<CustomerDto> GetTerritoryByRegionId(int id)
        {
            try
            {
                var region = this.regionService.GetById(id);
                if (region != null)
                {
                    var territories = this.territoryService.GetTerritoriesByRegionId(id);
                    return Ok(this.mapper.Map<TerritoryDb[], TerritoryDto[]>(territories));
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
        public ActionResult<RegionDto> Create(RegionDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<RegionDto, RegionDb>(model);
                    var region = this.regionService.Create(mappedModel);
                    return Ok(this.mapper.Map<RegionDb, RegionDto>(region));
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
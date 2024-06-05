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
    public class TerritoriesController : ControllerBase
    {
        private readonly TerritoryService territoryService;
        private readonly RegionService regionService;
        private readonly EmployeeTerritoryService employeeTerritoryService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<TerritoriesController> logger;

        public TerritoriesController(TerritoryService territoryService, EmployeeTerritoryService employeeTerritoryService, RegionService regionService, PagingService pagingService, IMapper mapper, ILogger<TerritoriesController> logger)
        {
            this.territoryService = territoryService;
            this.regionService = regionService;
            this.employeeTerritoryService = employeeTerritoryService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<TerritoryDto[]> GetAll()
        {
            try
            {
                var territories = this.territoryService.GetAll();
                return Ok(this.mapper.Map<TerritoryDb[], TerritoryDto[]>(territories));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all territories or a page of territories based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the territories. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of territories to fetch. If this parameter is not provided, all territories are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the territories by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedTerritories")]
        public ActionResult<PagedResultDto<TerritoryDto>> GetAllTerritories(int? skip, int? top, string? orderBy)
        {
            try
            {
                // Retrieve all territories
                var territories = this.territoryService.GetAll();

                // Get paged data
                var pagedResult = pagingService.GetPagedData<TerritoryDb, TerritoryDto>(territories, skip, top, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TerritoryDto> GetById(string id)
        {
            try
            {
                var teritory = this.territoryService.GetById(id);
                if (teritory != null)
                {
                    return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(teritory));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Employees")]
        public ActionResult<EmployeeDto[]> GetEmployeesByTerritory(string id)
        {
            try
            {
                var employees = this.employeeTerritoryService.GetEmployeesByTerritoryId(id);
                if (employees == null)
                {
                    return NotFound($"No employees for territory {id}");
                }

                return Ok(this.mapper.Map<EmployeeDb[], EmployeeDto[]>(employees));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Region")]
        public ActionResult<RegionDto[]> GetRegionByTerritory(string id)
        {
            try
            {
                var territory = this.territoryService.GetById(id);
                if (territory != null)
                {
                    var region = this.regionService.GetById(territory.RegionId ?? default);

                    if (region != null)
                    {
                        return Ok(this.mapper.Map<RegionDb, RegionDto>(region));
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
        public ActionResult<TerritoryDto> Create(TerritoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<TerritoryDto, TerritoryDb>(model);
                    var teritory = this.territoryService.Create(mappedModel);
                    return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(teritory));
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<TerritoryDto> Update(TerritoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<TerritoryDto, TerritoryDb>(model);
                    var territory = this.territoryService.Update(mappedModel);

                    if (territory != null)
                    {
                        return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(territory));
                    }

                    return NotFound();
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<TerritoryDto> Delete(string id)
        {
            try
            {
                var territory = this.territoryService.Delete(id);
                if (territory != null)
                {
                    return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(territory));
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
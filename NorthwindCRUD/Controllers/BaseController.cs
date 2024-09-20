using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Controllers
{
    public class BaseController<TDto, TDb, TId> : ControllerBase
        where TDto : class, IBaseDto
        where TDb : class, IBaseDb, new()
    {
        private readonly BaseDbService<TDto, TDb, string> baseDbService;
        private readonly ILogger logger;

        public BaseController(BaseDbService<TDto, TDb, string> baseDbService, ILogger logger)
        {
            this.baseDbService = baseDbService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<TDto[]> GetAll()
        {
            try
            {
                var result = this.baseDbService.GetAll();
                return Ok(result);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all customers or a page of customers based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the customers. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of customers to fetch. If this parameter is not provided, all customers are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the customers by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetWithSkip")]
        public ActionResult<PagedResultDto<CustomerDto>> GetWithSkip(
            [FromQuery][Attributes.SwaggerSkipParameter][Range(1, int.MaxValue, ErrorMessage = "Skip must be greater than 0.")] int? skip,
            [FromQuery][Attributes.SwaggerTopParameter] int? top,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                var pagedResult = this.baseDbService.GetWithPageSkip(skip, top, null, null, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all customers or a page of customers based on the provided parameters.
        /// </summary>
        /// <param name="pageIndex">The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).</param>
        /// <param name="size">The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the records by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetWithPage")]
        public ActionResult<PagedResultDto<CustomerDto>> GetWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                var pagedResult = this.baseDbService.GetWithPageSkip(null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<TDto>> UpsertAsync(TDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await this.baseDbService.Upsert(model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<TDto> GetById(TId id)
        {
            try
            {
                var result = this.baseDbService.GetById(id);

                if (result != null)
                {
                    return result;
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves the total number of customers.
        /// </summary>
        /// <returns>Total count of customers as an integer.</returns>
        [HttpGet("GetCount")]
        public ActionResult<CountResultDto> GetCustomersCount()
        {
            return this.baseDbService.GetCount();
        }
    }
}

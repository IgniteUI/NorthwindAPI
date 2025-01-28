using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseNorthwindAPIController<TDto, TDb, TId> : ControllerBase
        where TDto : class, IBaseDto
        where TDb : class, IBaseDb, new()
    {
        private readonly BaseDbService<TDto, TDb, TId> baseDbService;

        public BaseNorthwindAPIController(BaseDbService<TDto, TDb, TId> baseDbService)
        {
            this.baseDbService = baseDbService;
        }

        [HttpGet]
        public ActionResult<TDto[]> GetAll()
        {
            var result = this.baseDbService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetAllAuthorized")]
        [Authorize]
        public ActionResult<TDto[]> GetAllAuthorized()
        {
            var result = this.baseDbService.GetAll();
            return Ok(result);
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
            [FromQuery][Attributes.SwaggerSkipParameter][Range(0, int.MaxValue, ErrorMessage = "Skip must be greater than 0.")] int? skip,
            [FromQuery][Attributes.SwaggerTopParameter] int? top,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            var pagedResult = this.baseDbService.GetWithPageSkip(skip, top, null, null, orderBy);

            return Ok(pagedResult);
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
            var pagedResult = this.baseDbService.GetWithPageSkip(null, null, pageIndex, size, orderBy);
            return Ok(pagedResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TDto>> Create(TDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await this.baseDbService.Create(model);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<TDto>> Update(TDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await this.baseDbService.Update(model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<TDto> GetById(TId id)
        {
            var result = this.baseDbService.GetById(id);

            if (result != null)
            {
                return result;
            }

            return NotFound();
        }

        /// <summary>
        /// Retrieves the total number of entities.
        /// </summary>
        /// <returns>Total count of entities as an integer.</returns>
        [HttpGet("GetCount")]
        public ActionResult<CountResultDto> GetCount()
        {
            return this.baseDbService.GetCount();
        }

        /// <summary>
        /// Retrieves the total number of entities.
        /// </summary>
        /// <returns>Total count of entities as an integer.</returns>
        [HttpGet("GetCountAuthorized")]
        [Authorize]
        public ActionResult<CountResultDto> GetCountAuthorized()
        {
            return this.baseDbService.GetCount();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<TDto> Delete(TId id)
        {
            var product = this.baseDbService.Delete(id);
            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }
    }
}

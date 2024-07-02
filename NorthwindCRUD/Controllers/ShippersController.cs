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
    public class ShippersController : ControllerBase
    {
        private readonly ShipperService shipperService;
        private readonly OrderService orderService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<ShippersController> logger;

        public ShippersController(ShipperService shipperService, OrderService orderService, PagingService pagingService, IMapper mapper, ILogger<ShippersController> logger)
        {
            this.shipperService = shipperService;
            this.orderService = orderService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<ShipperDto[]> GetAll()
        {
            try
            {
                var shippers = this.shipperService.GetAll();
                return Ok(this.mapper.Map<ShipperDb[], ShipperDto[]>(shippers));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all shippers or a page of shippers based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the shippers. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of shippers to fetch. If this parameter is not provided, all shippers are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the shippers by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedShippersWithSkip")]
        public ActionResult<PagedResultDto<ShipperDto>> GetPagedShippersWithSkip(
            [FromQuery][SwaggerParameter("The number of records to skip before starting to fetch the shippers. If this parameter is not provided, fetching starts from the beginning.")] int? skip,
            [FromQuery][SwaggerParameter("The maximum number of shippers to fetch. If this parameter is not provided, all shippers are fetched.")] int? top,
            [FromQuery][SwaggerParameter("A comma-separated list of fields to order the shippers by, along with the sort direction (e.g., 'field1 asc, field2 desc').")] string? orderBy)
        {
            try
            {
                // Retrieve all shippers
                var shippers = this.shipperService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<ShipperDb, ShipperDto>(shippers, skip, top, null, null, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all shippers or a page of shippers based on the provided parameters.
        /// </summary>
        /// <param name="pageIndex">The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).</param>
        /// <param name="size">The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the records by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedShippersWithPage")]
        public ActionResult<PagedResultDto<ShipperDto>> GetPagedShippersWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve shippers as Queryable
                var shippers = this.shipperService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<RegionDb, RegionDto>(shippers, null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ShipperDto> GetById(int id)
        {
            try
            {
                var category = this.shipperService.GetById(id);
                if (category != null)
                {
                    return Ok(this.mapper.Map<ShipperDb, ShipperDto>(category));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Orders")]
        public ActionResult<OrderDto[]> GetOrdersByShipperId(int id)
        {
            try
            {
                var orders = this.orderService.GetOrdersByShipperId(id);
                return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(orders));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<ShipperDto> Create(ShipperDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<ShipperDto, ShipperDb>(model);
                    var shipper = this.shipperService.Create(mappedModel);
                    return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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
        public ActionResult<ShipperDto> Update(ShipperDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<ShipperDto, ShipperDb>(model);
                    var shipper = this.shipperService.Update(mappedModel);

                    if (shipper != null)
                    {
                        return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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
        public ActionResult<ShipperDto> Delete(int id)
        {
            try
            {
                var shipper = this.shipperService.Delete(id);
                if (shipper != null)
                {
                    return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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
namespace NorthwindCRUD.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly CustomerService customerService;
        private readonly OrderService orderService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<CustomersController> logger;

        public CustomersController(CustomerService customerService, OrderService orderService, PagingService pagingService, IMapper mapper, ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.orderService = orderService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<CustomerDto[]> GetAll()
        {
            try
            {
                var customers = this.customerService.GetAll();
                return Ok(this.mapper.Map<CustomerDb[], CustomerDto[]>(customers));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves all customers along with their associated orders.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an array of <see cref="CustomerDto"/> objects.
        /// </returns>
        public ActionResult<CustomerDto[]> GetAllCustomersWithOrders()
        {
            try
            {
                var customers = this.customerService.GetAllCustomerOrders();
                return Ok(this.mapper.Map<CustomerDb[], CustomerDto[]>(customers));
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
        [HttpGet("GetCustomersWithSkip")]
        public ActionResult<PagedResultDto<CustomerDto>> GetCustomersWithSkip(
            [FromQuery][Attributes.SwaggerSkipParameter][Range(0, int.MaxValue)] int? skip,
            [FromQuery][Attributes.SwaggerTopParameter][Range(0, int.MaxValue)] int? top,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve all customers
                var customers = this.customerService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<CustomerDb, CustomerDto>(customers, skip, top, null, null, orderBy);

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
        [HttpGet("GetCustomersWithPage")]
        public ActionResult<PagedResultDto<CustomerDto>> GetCustomersWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve customers as Queryable
                var customers = this.customerService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<CustomerDb, CustomerDto>(customers, null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
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
        [HttpGet("GetCustomersCount")]
        public ActionResult<CountResultDto> GetCustomersCount()
        {
            try
            {
                var count = customerService.GetAllAsQueryable().Count();
                return new CountResultDto() { Count = count };
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CustomerDto> GetById(string id)
        {
            try
            {
                var customer = this.customerService.GetById(id);

                if (customer != null)
                {
                    return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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
        public ActionResult<OrderDto[]> GetOrdersByCustomerId(string id)
        {
            try
            {
                var orders = this.orderService.GetOrdersByCustomerId(id);
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
        public ActionResult<CustomerDto> Create(CustomerDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CustomerDto, CustomerDb>(model);
                    var customer = this.customerService.Create(mappedModel);
                    return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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
        public ActionResult<CustomerDto> Update(CustomerDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CustomerDto, CustomerDb>(model);
                    var customer = this.customerService.Update(mappedModel);

                    if (customer != null)
                    {
                        return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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
        public ActionResult<CustomerDto> Delete(string id)
        {
            try
            {
                var customer = this.customerService.Delete(id);
                if (customer != null)
                {
                    return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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

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
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly EmployeeService employeeService;
        private readonly CustomerService customerService;
        private readonly ShipperService shipperService;
        private readonly ProductService productService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(OrderService orderService, EmployeeService employeeService, CustomerService customerService, ShipperService shipperService, ProductService productService, PagingService pagingService, IMapper mapper, ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.employeeService = employeeService;
            this.customerService = customerService;
            this.shipperService = shipperService;
            this.productService = productService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<OrderDto[]> GetAll()
        {
            try
            {
                var orders = this.orderService.GetAll();
                return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(orders));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all orders or a page of orders based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the orders. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of orders to fetch. If this parameter is not provided, all orders are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the orders by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedOrders")]
        public ActionResult<PagedResultDto<OrderDto>> GetAllOrders(
            [FromQuery][SwaggerParameter("The number of records to skip before starting to fetch the orders. If this parameter is not provided, fetching starts from the beginning.")] int? skip,
            [FromQuery][SwaggerParameter("The maximum number of orders to fetch. If this parameter is not provided, all orders are fetched.")] int? top,
            [FromQuery][SwaggerParameter("A comma-separated list of fields to order the orders by, along with the sort direction (e.g., 'field1 asc, field2 desc').")] string? orderBy)
        {
            try
            {
                // Retrieve all orders
                var orders = this.orderService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<OrderDb, OrderDto>(orders, skip, top, null, null, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all orders or a page of orders based on the provided parameters.
        /// </summary>
        /// <param name="pageIndex">The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).</param>
        /// <param name="size">The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the records by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedOrdersWithPage")]
        public ActionResult<PagedResultDto<OrderDto>> GetPagedOrdersWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve orders as Queryable
                var orders = this.orderService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<OrderDb, OrderDto>(orders, null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetById(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);

                if (order != null)
                {
                    return Ok(this.mapper.Map<OrderDb, OrderDto>(order));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Details")]
        public ActionResult<OrderDetailDto[]> GetDetailsByOrderId(int id)
        {
            try
            {
                var orderDetail = this.orderService.GetOrderDetailsById(id);
                if (orderDetail != null)
                {
                    return Ok(this.mapper.Map<OrderDetailDb[], OrderDetailDto[]>(orderDetail));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Customer")]
        public ActionResult<CustomerDto> GetCustomerByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null && order.CustomerId != null)
                {
                    var customer = this.customerService.GetById(order.CustomerId);

                    if (customer != null)
                    {
                        return this.mapper.Map<CustomerDb, CustomerDto>(customer);
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

        [HttpGet("{id}/Employee")]
        public ActionResult<CustomerDto> GetEmployeeByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null)
                {
                    var employee = this.employeeService.GetById(order.EmployeeId ?? default);
                    if (employee != null)
                    {
                        return Ok(this.mapper.Map<EmployeeDb, EmployeeDto>(employee));
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

        [HttpGet("{id}/Shipper")]
        public ActionResult<CustomerDto> GetShipperByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null)
                {
                    var shipper = this.shipperService.GetById(order.ShipVia);

                    if (shipper != null)
                    {
                        return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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

        [HttpGet("{id}/Products")]
        public ActionResult<ProductDto[]> GetProductsByOrderId(int id)
        {
            try
            {
                var orderDetails = this.orderService.GetOrderDetailsById(id);
                if (orderDetails != null)
                {
                    var productIds = orderDetails.Select(o => o.ProductId).ToArray();
                    var products = this.productService.GetProductsByIds(productIds);

                    if (products != null)
                    {
                        var productDtos = this.mapper.Map<ProductDb[], ProductDto[]>(products);
                        return Ok(productDtos);
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

        [HttpGet("retrieve/{ordersToRetrieve}")]
        [Authorize]
        public ActionResult<OrderDto[]> OrdersToRetrieve(int ordersToRetrieve)
        {
            try
            {
                var orders = this.orderService.GetNOrders(ordersToRetrieve);
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
        public ActionResult<OrderDto> Create(OrderDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<OrderDto, OrderDb>(model);
                    var order = this.orderService.Create(mappedModel);
                    return Ok(this.mapper.Map<OrderDb, OrderDto>(order));
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
        public ActionResult<OrderDto> Update(OrderDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<OrderDto, OrderDb>(model);
                    var order = this.orderService.Update(mappedModel);
                    if (order != null)
                    {
                        return Ok(this.mapper.Map<OrderDb, OrderDto>(order));
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
        public ActionResult<OrderDto> Delete(int id)
        {
            try
            {
                var order = this.orderService.Delete(id);
                if (order != null)
                {
                    return Ok(this.mapper.Map<OrderDb, OrderDto>(order));
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

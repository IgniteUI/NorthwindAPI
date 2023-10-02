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
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly EmployeeService employeeService;
        private readonly CustomerService customerService;
        private readonly ShipperService shipperService;
        private readonly ProductService productService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrdersController(OrderService orderService, EmployeeService employeeService, CustomerService customerService, ShipperService shipperService, ProductService productService, IMapper mapper, ILogger logger)
        {
            this.orderService = orderService;
            this.employeeService = employeeService;
            this.customerService = customerService;
            this.shipperService = shipperService;
            this.productService = productService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
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
        
        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
        public ActionResult<OrderDetailDto[]> GetDetailsByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null)
                {
                    return Ok(this.mapper.Map<OrderDetailDb[], OrderDetailDto[]>(order.Details.ToArray()));
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
        [Authorize]
        public ActionResult<CustomerDto> GetCustomerByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null)
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
        [Authorize]
        public ActionResult<CustomerDto> GetEmployeeByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null)
                {
                    var employee = this.employeeService.GetById(order.EmployeeId);
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
        [Authorize]
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
        [Authorize]
        public ActionResult<ProductDto[]> GetProductsByOrderId(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);
                if (order != null)
                {
                    var productIds = order.Details.Select(o => o.ProductId).ToArray();
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

namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CustomersController : BaseController<CustomerDto, CustomerDb, string>
    {
        private readonly OrderService orderService;

        public CustomersController(CustomerService customerService, OrderService orderService, PagingService pagingService, IMapper mapper, ILogger<CustomersController> logger)
            : base(customerService, logger)
        {
            this.orderService = orderService;
        }

        [HttpGet("{id}/Orders")]
        public ActionResult<OrderDto[]> GetOrdersByCustomerId(string id)
        {
            var result = this.orderService.GetOrdersByCustomerId(id);
            return Ok(result);
        }
    }
}
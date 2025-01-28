namespace NorthwindCRUD.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class OrdersController : BaseNorthwindAPIController<OrderDto, OrderDb, int>
    {
        private readonly OrderService orderService;
        private readonly EmployeeService employeeService;
        private readonly CustomerService customerService;
        private readonly ShipperService shipperService;
        private readonly ProductService productService;

        public OrdersController(OrderService orderService, EmployeeService employeeService, CustomerService customerService, ShipperService shipperService, ProductService productService)
            : base(orderService)
        {
            this.orderService = orderService;
            this.employeeService = employeeService;
            this.customerService = customerService;
            this.shipperService = shipperService;
            this.productService = productService;
        }

        [HttpGet("{id}/Details")]
        public ActionResult<OrderDetailDto[]> GetDetailsByOrderId(int id)
        {
            var orderDetail = this.orderService.GetOrderDetailsById(id);
            if (orderDetail != null)
            {
                return Ok(orderDetail);
            }

            return NotFound();
        }

        [HttpGet("{id}/Customer")]
        public ActionResult<CustomerDto> GetCustomerByOrderId(int id)
        {
            var order = this.orderService.GetById(id);
            if (order != null && order.CustomerId != null)
            {
                var customer = this.customerService.GetById(order.CustomerId);

                if (customer != null)
                {
                    return customer;
                }
            }

            return NotFound();
        }

        [HttpGet("{id}/Employee")]
        public ActionResult<CustomerDto> GetEmployeeByOrderId(int id)
        {
            var order = this.orderService.GetById(id);
            if (order != null)
            {
                var employee = this.employeeService.GetById(order.EmployeeId);
                if (employee != null)
                {
                    return Ok(employee);
                }
            }

            return NotFound();
        }

        [HttpGet("{id}/Shipper")]
        public ActionResult<CustomerDto> GetShipperByOrderId(int id)
        {
            var order = this.orderService.GetById(id);
            if (order != null)
            {
                var shipper = this.shipperService.GetById(id);

                if (shipper != null)
                {
                    return Ok(shipper);
                }
            }

            return NotFound();
        }

        [HttpGet("{id}/Products")]
        public ActionResult<ProductDto[]> GetProductsByOrderId(int id)
        {
            var orderDetails = this.orderService.GetOrderDetailsById(id);
            if (orderDetails != null)
            {
                var productIds = orderDetails.Select(o => o.ProductId).ToArray();
                var products = this.productService.GetProductsByIds(productIds);

                if (products != null)
                {
                    return Ok(products);
                }
            }

            return NotFound();
        }

        [HttpGet("retrieve/{ordersToRetrieve}")]
        [Authorize]
        public ActionResult<OrderDto[]> OrdersToRetrieve(int ordersToRetrieve)
        {
            var orders = this.orderService.GetNOrders(ordersToRetrieve);
            return Ok(orders);
        }
    }
}

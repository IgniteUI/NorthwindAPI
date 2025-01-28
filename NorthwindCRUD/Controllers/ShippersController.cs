namespace NorthwindCRUD.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class ShippersController : BaseNorthwindAPIController<ShipperDto, ShipperDb, int>
    {
        private readonly OrderService orderService;

        public ShippersController(ShipperService shipperService, OrderService orderService)
            : base(shipperService)
        {
            this.orderService = orderService;
        }

        [HttpGet("{id}/Orders")]
        public ActionResult<OrderDto[]> GetOrdersByShipperId(int id)
        {
            var orders = this.orderService.GetOrdersByShipperId(id);
            return Ok(orders);
        }
    }
}
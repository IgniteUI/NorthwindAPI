namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly IMapper mapper;

        public OrderController(OrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<OrderInputModel[]> GetAll()
        {
            var orders = this.orderService.GetAll();
            return Ok(this.mapper.Map<OrderDb[], OrderInputModel[]>(orders));
        }

        [HttpGet("{id}")]
        public ActionResult<OrderInputModel> GetById(int id)
        {
            var order = this.orderService.GetById(id);

            if (order != null)
            {
                return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<OrderInputModel> Create(OrderInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<OrderInputModel, OrderDb>(model);
                var order = this.orderService.Create(mappedModel);
                return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public ActionResult<OrderInputModel> Update(OrderInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<OrderInputModel, OrderDb>(model);
                var order = this.orderService.Update(mappedModel);
                return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public ActionResult<OrderInputModel> Delete(int id)
        {
            var order = this.orderService.Delete(id);

            if (order != null)
            {
                return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
            }

            return NotFound();
        }
    }
}

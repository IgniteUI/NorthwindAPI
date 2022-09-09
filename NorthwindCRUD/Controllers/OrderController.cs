namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;
    using System.Web.Http.ModelBinding;

    [ApiController]
    [Route("[controller]")]
    public class OrderController
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
            return this.mapper.Map<OrderDb[], OrderInputModel[]>(orders);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderInputModel> GetById(int id)
        {
            var order = this.orderService.GetById(id);
            return this.mapper.Map<OrderDb, OrderInputModel>(order);
        }

        [HttpPost]
        public ActionResult<OrderInputModel> Create(OrderInputModel model)
        {
            var mappedModel = this.mapper.Map<OrderInputModel, OrderDb>(model);
            var order = this.orderService.Create(mappedModel);
            return this.mapper.Map<OrderDb, OrderInputModel>(order);
        }

        [HttpPut]
        public ActionResult<OrderInputModel> Update(OrderInputModel model)
        {
            var mappedModel = this.mapper.Map<OrderInputModel, OrderDb>(model);
            var order = this.orderService.Update(mappedModel);
            return this.mapper.Map<OrderDb, OrderInputModel>(order);
        }

        [HttpDelete("{id}")]
        public ActionResult<OrderInputModel> Delete(int id)
        {
            var order = this.orderService.Delete(id);
            return this.mapper.Map<OrderDb, OrderInputModel>(order);
        }
    }
}

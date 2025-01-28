using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Controllers
{
    [GraphRoute("order")]
    public class OrderGraphController : GraphController
    {
        private readonly OrderService orderService;

        public OrderGraphController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        [Query]
        public OrderDto[] GetAll()
        {
            var orders = this.orderService.GetAll();
            return orders;
        }

        [Query]
        public OrderDto? GetById(int id)
        {
            var order = this.orderService.GetById(id);

            if (order != null)
            {
                return order;
            }

            return null;
        }

        [Mutation]
        public async Task<OrderDto> Create(OrderDto model)
        {
            var order = await this.orderService.Create(model);
            return order;
        }

        [Mutation]
        public async Task<OrderDto?> Update(OrderDto model)
        {
            var order = await orderService.Update(model, model.OrderId);
            return order;
        }

        [Mutation]
        public OrderDto? Delete(int id)
        {
            var order = this.orderService.Delete(id);
            return order;
        }
    }
}

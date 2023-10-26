using AutoMapper;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Controllers
{
    [GraphRoute("order")]
    public class OrderGraphController : GraphController
    {
        private readonly OrderService orderService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrderGraphController(OrderService orderService, IMapper mapper, ILogger logger)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Query]
        public OrderDto[] GetAll()
        {
            var orders = this.orderService.GetAll();
            return this.mapper.Map<OrderDb[], OrderDto[]>(orders);
        }

        [Query]
        public OrderDto? GetById(int id)
        {
            var order = this.orderService.GetById(id);

            if (order != null)
            {
                return this.mapper.Map<OrderDb, OrderDto>(order);
            }

            return null;
        }

        [Mutation]
        public OrderDto Create(OrderDto model)
        {
            var mappedModel = this.mapper.Map<OrderDto, OrderDb>(model);
            var order = this.orderService.Create(mappedModel);
            return this.mapper.Map<OrderDb, OrderDto>(order);
        }

        [Mutation]
        public OrderDto? Update(OrderDto model)
        {
            var mappedModel = this.mapper.Map<OrderDto, OrderDb>(model);
            var order = this.orderService.Update(mappedModel);
            return order != null ? this.mapper.Map<OrderDb, OrderDto>(order) : null;
        }

        [Mutation]
        public OrderDto? Delete(int id)
        {
            var order = this.orderService.Delete(id);

            if (order != null)
            {
                return this.mapper.Map<OrderDb, OrderDto>(order);
            }

            return null;
        }
    }
}

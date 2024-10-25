using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class OrderService : BaseDbService<OrderDto, OrderDb, int>
    {
        private readonly IMapper mapper;
        private readonly DataContext dataContext;

        public OrderService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        public OrderDto[] GetNOrders(int numberOfOrdersToRetrieve)
        {
            var oreders = this.dataContext.Orders
                .Include(c => c.ShipAddress)
                .Take(numberOfOrdersToRetrieve)
                .ToArray();

            return this.mapper.Map<OrderDto[]>(oreders);
        }

        public OrderDetailDto[] GetOrderDetailsById(int id)
        {
            var details = this.dataContext.OrderDetails.Where(o => o.OrderId == id).ToArray();
            return this.mapper.Map<OrderDetailDto[]>(details);
        }

        public OrderDto[] GetOrdersByCustomerId(string id)
        {
            return mapper.Map<OrderDto[]>(this.GetAllAsQueryable()
                .Where(o => o.CustomerId == id)
                .ToArray());
        }

        public OrderDto[] GetOrdersByEmployeeId(int id)
        {
            var oders = this.GetAllAsQueryable()
                .Where(o => o.EmployeeId == id)
                .ToArray();

            return this.mapper.Map<OrderDto[]>(oders);
        }

        public OrderDto[] GetOrdersByShipperId(int id)
        {
            var oreders = this.GetAllAsQueryable()
                .Where(o => o.ShipVia == id)
                .ToArray();

            return this.mapper.Map<OrderDto[]>(oreders);
        }

        public OrderDetailDto[] GetOrderDetailsByProductId(int id)
        {
            var details = this.dataContext.OrderDetails
                .Where(o => o.ProductId == id)
                .ToArray();

            return this.mapper.Map<OrderDetailDto[]>(details);
        }
    }
}

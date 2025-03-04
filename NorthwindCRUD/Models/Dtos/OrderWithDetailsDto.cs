using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class OrderWithDetailsDto : OrderDto, IOrder
    {
        public OrderDetailDto[] OrderDetails { get; set; }
    }
}

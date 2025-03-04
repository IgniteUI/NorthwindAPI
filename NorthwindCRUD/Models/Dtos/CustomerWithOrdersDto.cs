using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class CustomerWithOrdersDto : CustomerDto, ICustomer
    {
        public OrderWithDetailsDto[] Orders { get; set; }
    }
}

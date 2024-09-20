using AutoMapper;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class CustomerService : BaseDbService<CustomerDto, CustomerDb, string>
    {
        public CustomerService(DataContext dataContext, IMapper mapper, IPagingService pagingService)
            : base(dataContext, mapper, pagingService) { }
    }
}

using AutoMapper;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class SupplierService : BaseDbService<SupplierDto, SupplierDb, int>
    {
        public SupplierService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
        }
    }
}

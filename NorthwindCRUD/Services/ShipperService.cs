using AutoMapper;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class ShipperService : BaseDbService<ShipperDto, ShipperDb, int>
    {
        public ShipperService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
        }
    }
}

using AutoMapper;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class TerritoryService : BaseDbService<TerritoryDto, TerritoryDb, string>
    {
        public TerritoryService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
        }

        public TerritoryDto[] GetTerritoriesByRegionId(int id)
        {
            var territories = this.GetAll().Where(t => t.RegionId == id).ToArray();
            return territories;
        }
    }
}

namespace NorthwindCRUD.Services
{
    using AutoMapper;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;

    public class CategoryService : BaseDbService<CategoryDto, CategoryDb, int>
    {
        public CategoryService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
        }

        public CategoryDetailsDto GetDetailsById(int id)
        {
            var category = this.GetDbById(id);
            return this.mapper.Map<CategoryDetailsDto>(category);
        }
    }
}

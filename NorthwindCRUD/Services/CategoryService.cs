namespace NorthwindCRUD.Services
{
    using AutoMapper;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;

    public class CategoryService : BaseDbService<CategoryDto, CategoryDb, int>
    {
        private readonly IMapper mapper;

        public CategoryService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
            this.mapper = mapper;
        }

        public CategoryDetailsDto GetDetailsById(int id)
        {
            var category = this.GetDbById(id);

            return mapper.Map<CategoryDetailsDto>(category);
        }
    }
}

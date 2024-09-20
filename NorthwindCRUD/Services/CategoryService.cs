namespace NorthwindCRUD.Services
{
    using AutoMapper;
    using NorthwindCRUD.Helpers;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;

    public class CategoryService : BaseDbService<CategoryDto, CategoryDb>
    {
        private readonly DataContext dataContext;
        private readonly IPagingService pagingService;

        public CategoryService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
            this.dataContext = dataContext;
        }

        public CategoryDb Create(CategoryDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }

            model.CategoryId = id;

            PropertyHelper<CategoryDb>.MakePropertiesEmptyIfNull(model);

            var categoryEntity = this.dataContext.Categories.Add(model);
            this.dataContext.SaveChanges();

            return categoryEntity.Entity;
        }
    }
}

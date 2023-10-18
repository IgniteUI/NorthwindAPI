namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [GraphRoute("category")]
    public class CategoryGraphController : GraphController
    {
        private readonly CategoryService categoryService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CategoryGraphController(CategoryService categoryService, IMapper mapper, ILogger logger)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.logger = logger;   
        }

        [Query]
        public CategoryDto[] GetAll()
        {
            var categories = this.categoryService.GetAll();
            return this.mapper.Map<CategoryDb[], CategoryDto[]>(categories);
        }

        [Query]
        public CategoryDto? GetById(int id)
        {
            var category = this.categoryService.GetById(id);

            if (category != null)
            {
                return this.mapper.Map<CategoryDb, CategoryDto>(category);
            }

            return null;
        }

        [Mutation]
        public CategoryDto Create(CategoryDto model)
        {
            var mappedModel = this.mapper.Map<CategoryDto, CategoryDb>(model);
            var category = this.categoryService.Create(mappedModel);
            return this.mapper.Map<CategoryDb, CategoryDto>(category);
        }

        [Mutation]
        public CategoryDto Update(CategoryDto model)
        {
            var mappedModel = this.mapper.Map<CategoryDto, CategoryDb>(model);
            var category = this.categoryService.Update(mappedModel);
            return this.mapper.Map<CategoryDb, CategoryDto>(category);
        }

        [Mutation]
        public CategoryDto Delete(int id)
        {
            var category = this.categoryService.Delete(id);

            if (category != null)
            {
                return this.mapper.Map<CategoryDb, CategoryDto>(category);
            }

            return null;
        }
    }
}
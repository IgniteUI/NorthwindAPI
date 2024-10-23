namespace NorthwindCRUD.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [GraphRoute("category")]
    public class CategoryGraphController : GraphController
    {
        private readonly CategoryService categoryService;

        public CategoryGraphController(CategoryService categoryService, IMapper mapper, ILogger logger)
        {
            this.categoryService = categoryService;
        }

        [Query]
        public CategoryDto[] GetAll()
        {
            var categories = this.categoryService.GetAll();
            return categories;
        }

        [Query]
        public CategoryDto? GetById(int id)
        {
            var category = this.categoryService.GetById(id);

            if (category != null)
            {
                return category;
            }

            return null;
        }

        [Mutation]
        public async Task<CategoryDto> Create(CategoryDto model)
        {
            var category = await this.categoryService.Create(model);
            return category;
        }

        [Mutation]
        public async Task<CategoryDto?> Update(CategoryDto model)
        {
            var category = await this.categoryService.Update(model);
            return category;
        }

        [Mutation]
        public CategoryDto? Delete(int id)
        {
            var category = this.categoryService.Delete(id);
            return category;
        }
    }
}
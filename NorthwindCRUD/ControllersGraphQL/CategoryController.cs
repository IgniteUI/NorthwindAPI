namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
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
        public CategoryInputModel[] GetAll()
        {
            var categories = this.categoryService.GetAll();
            return this.mapper.Map<CategoryDb[], CategoryInputModel[]>(categories);
        }

        [Query]
        public CategoryInputModel? GetById(int id)
        {
            var category = this.categoryService.GetById(id);

            if (category != null)
            {
                return this.mapper.Map<CategoryDb, CategoryInputModel>(category);
            }

            return null;
        }

        [Mutation]
        public CategoryInputModel Create(CategoryInputModel model)
        {
            var mappedModel = this.mapper.Map<CategoryInputModel, CategoryDb>(model);
            var category = this.categoryService.Create(mappedModel);
            return this.mapper.Map<CategoryDb, CategoryInputModel>(category);
        }

        [Mutation]
        public CategoryInputModel Update(CategoryInputModel model)
        {
            var mappedModel = this.mapper.Map<CategoryInputModel, CategoryDb>(model);
            var category = this.categoryService.Update(mappedModel);
            return this.mapper.Map<CategoryDb, CategoryInputModel>(category);
        }

        [Mutation]
        public CategoryInputModel Delete(int id)
        {
            var category = this.categoryService.Delete(id);

            if (category != null)
            {
                return this.mapper.Map<CategoryDb, CategoryInputModel>(category);
            }

            return null;
        }
    }
}
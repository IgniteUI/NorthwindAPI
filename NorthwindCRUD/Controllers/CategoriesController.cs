namespace NorthwindCRUD.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : BaseNorthwindAPIController<CategoryDto, CategoryDb, int>
    {
        private readonly CategoryService categoryService;
        private readonly ProductService productService;

        public CategoriesController(CategoryService categoryService, ProductService productService)
                 : base(categoryService)
        {
            this.categoryService = categoryService;
            this.productService = productService;
        }

        [HttpGet("{id}/Details")]
        public ActionResult<CategoryDetailsDto> GetDetailsById(int id)
        {
            var category = this.categoryService.GetDetailsById(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("{id}/Products")]
        public ActionResult<ProductDto[]> GetProductsByCategoryId(int id)
        {
            var products = this.productService.GetAllByCategoryId(id);
            return Ok(products);
        }
    }
}
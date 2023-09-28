namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService categoryService;
        private readonly ProductService productService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CategoriesController(CategoryService categoryService, ProductService productService, IMapper mapper, ILogger logger)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.mapper = mapper;
            this.logger = logger;   
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Models.Dtos.CategoryDto[]> GetAll()
        {
            try
            {
                var categories = this.categoryService.GetAll();
                return base.Ok(this.mapper.Map<CategoryDb[], CategoryDto[]>(categories));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
            
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CategoryDto> GetById(int id)
        {
            try
            {
                var category = this.categoryService.GetById(id);
                if (category != null)
                {
                    return base.Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }


        [HttpGet("{id}/Details")]
        [Authorize]
        public ActionResult<CategoryDetailsDto> GetDetailsById(int id)
        {
            try
            {
                var category = this.categoryService.GetById(id);
                if (category != null)
                {
                    return Ok(this.mapper.Map<CategoryDb, CategoryDetailsDto>(category));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Products")]
        [Authorize]
        public ActionResult<ProductDto[]> GetProductsByCategoryId(int id)
        {
            try
            {
                var products = this.productService.GetAllByCategoryId(id);
                if (products != null)
                {
                    return Ok(this.mapper.Map<ProductDb[], ProductDto[]>(products));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<CategoryDetailsDto> Create(CategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CategoryDto, CategoryDb>(model);
                    var category = this.categoryService.Create(mappedModel);
                    return Ok(this.mapper.Map<CategoryDb, CategoryDetailsDto>(category));
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<CategoryDto> Update(CategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CategoryDto, CategoryDb>(model);
                    var category = this.categoryService.Update(mappedModel);

                    if (category != null)
                    {
                        return base.Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
                    }

                    return NotFound();
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<CategoryDto> Delete(int id)
        {
            try
            {
                var category = this.categoryService.Delete(id);
                if (category != null)
                {
                    return base.Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
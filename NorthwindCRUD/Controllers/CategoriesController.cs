namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService categoryService;
        private readonly ProductService productService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(CategoryService categoryService, ProductService productService, PagingService pagingService, IMapper mapper, ILogger<CategoriesController> logger)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<CategoryDto[]> GetAll()
        {
            try
            {
                var categories = this.categoryService.GetAll();
                return Ok(this.mapper.Map<CategoryDb[], CategoryDto[]>(categories));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all categories or a page of categories based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the categories. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of categories to fetch. If this parameter is not provided, all categories are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the categories by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetCategoriesWithSkip")]
        public ActionResult<PagedResultDto<CategoryDto>> GetCategoriesWithSkip(
            [FromQuery][Attributes.SwaggerSkipParameter] int? skip,
            [FromQuery][Attributes.SwaggerTopParameter] int? top,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve categories as Queryable
                var categories = this.categoryService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<CategoryDb, CategoryDto>(categories, skip, top, null, null, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all categories or a page of categories based on the provided parameters.
        /// </summary>
        /// <param name="pageIndex">The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).</param>
        /// <param name="size">The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the records by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetCategoriesWithPage")]
        public ActionResult<PagedResultDto<CategoryDto>> GetCategoriesWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve categories as Queryable
                var categories = this.categoryService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<CategoryDb, CategoryDto>(categories, null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetById(int id)
        {
            try
            {
                var category = this.categoryService.GetById(id);
                if (category != null)
                {
                    return Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
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
        public ActionResult<ProductDto[]> GetProductsByCategoryId(int id)
        {
            try
            {
                var products = this.productService.GetAllByCategoryId(id);
                return Ok(this.mapper.Map<ProductDb[], ProductDto[]>(products));
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
                        return Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
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
                    return Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
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
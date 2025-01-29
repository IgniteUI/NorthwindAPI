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
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly OrderService orderService;
        private readonly SupplierService supplierService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(ProductService productService, CategoryService categoryService, OrderService orderService, SupplierService supplierService, PagingService pagingService, IMapper mapper, ILogger<ProductsController> logger)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
            this.supplierService = supplierService;
            this.pagingService = pagingService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<ProductDto[]> GetAll()
        {
            try
            {
                var products = this.productService.GetAll();
                return Ok(this.mapper.Map<ProductDb[], ProductDto[]>(products));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllAuthorized")]
        [Authorize]
        public ActionResult<OrderDto[]> GetAllAuthorized()
        {
            try
            {
                var products = this.productService.GetAll();
                return Ok(this.mapper.Map<ProductDb[], ProductDto[]>(products));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all products or a page of products based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the products. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of products to fetch. If this parameter is not provided, all products are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the products by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedProducts")]
        public ActionResult<PagedResultDto<ProductDto>> GetAllProducts(
            [FromQuery][Attributes.SwaggerSkipParameter] int? skip,
            [FromQuery][Attributes.SwaggerTopParameter] int? top,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve all products
                var products = this.productService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<ProductDb, ProductDto>(products, skip, top, null, null, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all products or a page of products based on the provided parameters.
        /// </summary>
        /// <param name="pageIndex">The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).</param>
        /// <param name="size">The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the records by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedProductsWithPage")]
        public ActionResult<PagedResultDto<ProductDto>> GetPagedProductsWithPage(
            [FromQuery][Attributes.SwaggerPageParameter] int? pageIndex,
            [FromQuery][Attributes.SwaggerSizeParameter] int? size,
            [FromQuery][Attributes.SwaggerOrderByParameter] string? orderBy)
        {
            try
            {
                // Retrieve products as Queryable
                var products = this.productService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedData<ProductDb, ProductDto>(products, null, null, pageIndex, size, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves the total number of products.
        /// </summary>
        /// <returns>Total count of products as an integer.</returns>
        [HttpGet("GetProductsCount")]
        public ActionResult<CountResultDto> GetProductsCount()
        {
            try
            {
                var count = productService.GetAllAsQueryable().Count();
                return new CountResultDto() { Count = count };
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves the total number of products.
        /// </summary>
        /// <returns>Total count of products as an integer.</returns>
        [HttpGet("GetProductsCountAuthorized")]
        public ActionResult<CountResultDto> GetProductsCountAuthorized()
        {
            try
            {
                var count = productService.GetAllAsQueryable().Count();
                return new CountResultDto() { Count = count };
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetById(int id)
        {
            try
            {
                var product = this.productService.GetById(id);
                if (product != null)
                {
                    return Ok(this.mapper.Map<ProductDb, ProductDto>(product));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Category")]
        public ActionResult<CategoryDto> GetCategoryByProductId(int id)
        {
            try
            {
                var product = this.productService.GetById(id);
                if (product != null)
                {
                    var category = this.categoryService.GetById(product.CategoryId ?? default);

                    if (category != null)
                    {
                        return Ok(this.mapper.Map<CategoryDb, CategoryDto>(category));
                    }
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/OrderDetails")]
        public ActionResult<OrderDetailDto[]> GetOrderDetailsByProductId(int id)
        {
            try
            {
                var product = this.productService.GetById(id);
                if (product != null)
                {
                    var orderDetails = this.orderService.GetOrderDetailsByProductId(id);
                    return Ok(this.mapper.Map<OrderDetailDb[], OrderDetailDto[]>(orderDetails));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Supplier")]
        public ActionResult<SupplierDto> GetSupplierByProductId(int id)
        {
            try
            {
                var product = this.productService.GetById(id);
                if (product != null)
                {
                    var supplier = this.supplierService.GetById(product.SupplierId ?? default);

                    if (supplier != null)
                    {
                        return Ok(this.mapper.Map<SupplierDb, SupplierDto>(supplier));
                    }
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
        public ActionResult<ProductDto> Create(ProductDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<ProductDto, ProductDb>(model);
                    var product = this.productService.Create(mappedModel);
                    return Ok(this.mapper.Map<ProductDb, ProductDto>(product));
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
               return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<ProductDto> Update(ProductDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<ProductDto, ProductDb>(model);
                    var product = this.productService.Update(mappedModel);

                    if (product != null)
                    {
                        return Ok(this.mapper.Map<ProductDb, ProductDto>(product));
                    }

                    return NotFound();
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<ProductDto> Delete(int id)
        {
            try
            {
                var product = this.productService.Delete(id);
                if (product != null)
                {
                    return Ok(this.mapper.Map<ProductDb, ProductDto>(product));
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
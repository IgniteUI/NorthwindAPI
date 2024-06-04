namespace NorthwindCRUD.Controllers
{
    using System.Globalization;
    using System.Reflection;
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

        /// <summary>
        /// Fetches all products or a page of products based on the provided parameters.
        /// </summary>
        /// <param name="skip">Previously called pageNumber. The number of the page to fetch. If this parameter is not provided, all products are fetched.</param>
        /// <param name="top">Previously called pageSize. The size of the page to fetch. If this parameter is not provided, all products are fetched.</param>
        /// <param name="orderBy">The fields to order by, in the format "field1 asc, field2 desc". If not provided, defaults to no specific order.</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetAllPagedProducts")]
        public ActionResult<PagedResultDto<ProductDto>> GetAllProducts(int? skip, int? top, string? orderBy)
        {
            try
            {
                // Retrieve all products
                var products = this.productService.GetAll();

                // Get paged data
                var pagedResult = pagingService.GetPagedData<ProductDb, ProductDto>(products, skip, top, orderBy);

                return Ok(pagedResult);
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
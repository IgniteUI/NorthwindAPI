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
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ProductsController(ProductService productService, CategoryService categoryService, OrderService orderService, SupplierService supplierService, IMapper mapper, ILogger logger)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
            this.supplierService = supplierService;
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
                        return Ok(this.mapper.Map<CategoryDb , CategoryDto>(category));
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
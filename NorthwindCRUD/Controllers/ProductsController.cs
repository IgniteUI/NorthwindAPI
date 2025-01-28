namespace NorthwindCRUD.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class ProductsController : BaseNorthwindAPIController<ProductDto, ProductDb, int>
    {
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly OrderService orderService;
        private readonly SupplierService supplierService;

        public ProductsController(ProductService productService, CategoryService categoryService, OrderService orderService, SupplierService supplierService)
            : base(productService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
            this.supplierService = supplierService;
        }

        [HttpGet("{id}/Category")]
        public ActionResult<CategoryDto> GetCategoryByProductId(int id)
        {
            var product = this.productService.GetById(id);
            if (product != null)
            {
                var category = this.categoryService.GetById(product.CategoryId ?? default);

                if (category != null)
                {
                    return category;
                }
            }

            return NotFound();
        }

        [HttpGet("{id}/OrderDetails")]
        public ActionResult<OrderDetailDto[]> GetOrderDetailsByProductId(int id)
        {
            var product = this.productService.GetById(id);
            if (product != null)
            {
                var orderDetails = this.orderService.GetOrderDetailsByProductId(id);
                return Ok(orderDetails);
            }

            return NotFound();
        }

        [HttpGet("{id}/Supplier")]
        public ActionResult<SupplierDto> GetSupplierByProductId(int id)
        {
            var product = this.productService.GetById(id);
            if (product != null)
            {
                var supplier = this.supplierService.GetById(product.SupplierId ?? default);

                if (supplier != null)
                {
                    return Ok(supplier);
                }
            }

            return NotFound();
        }
    }
}
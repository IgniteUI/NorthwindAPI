namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class SuppliersController : BaseNorthwindAPIController<SupplierDto, SupplierDb, int>
    {
        private readonly ProductService productService;

        public SuppliersController(SupplierService supplierService, ProductService productService, IMapper mapper, ILogger<SuppliersController> logger)
            : base(supplierService)
        {
            this.productService = productService;
        }

        [HttpGet("{id}/Products")]
        public ActionResult<ProductDto[]> GetProductsBySupplierId(int id)
        {
            var products = this.productService.GetAllBySupplierId(id);
            return Ok(products);
        }
    }
}
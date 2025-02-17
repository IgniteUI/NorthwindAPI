namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly BrandService brandsService;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesController> logger;

        public BrandsController(BrandService brandsService, IMapper mapper, ILogger<CategoriesController> logger)
        {
            this.brandsService = brandsService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<AssetDto[]> GetAll()
        {
            try
            {
                var brandSales = this.brandsService.GetAll();
                return Ok(this.mapper.Map<BrandSaleDb[], BrandSaleDto[]>(brandSales));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
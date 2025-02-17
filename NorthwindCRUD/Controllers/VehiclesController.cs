namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService vehiclesService;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesController> logger;

        public VehiclesController(VehicleService brandsService, IMapper mapper, ILogger<CategoriesController> logger)
        {
            this.vehiclesService = brandsService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<AssetDto[]> GetAll()
        {
            try
            {
                var vehicles = this.vehiclesService.GetAll();
                return Ok(this.mapper.Map<VehicleDb[], VehicleDto[]>(vehicles));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
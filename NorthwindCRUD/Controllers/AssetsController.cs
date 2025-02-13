namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly AssetService assetService;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesController> logger;

        public AssetsController(AssetService assetService, IMapper mapper, ILogger<CategoriesController> logger)
        {
            this.assetService = assetService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<AssetDto[]> GetAll()
        {
            try
            {
                var assets = this.assetService.GetAll();
                return Ok(this.mapper.Map<AssetDb[], AssetDto[]>(assets));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
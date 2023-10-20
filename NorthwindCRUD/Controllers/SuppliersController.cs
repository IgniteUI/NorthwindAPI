namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService supplierService;
        private readonly ProductService productService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public SuppliersController(SupplierService supplierService, ProductService productService, IMapper mapper, ILogger logger)
        {
            this.supplierService = supplierService;
            this.productService = productService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<SupplierDto[]> GetAll()
        {
            try
            {
                var suppliers = this.supplierService.GetAll();
                return Ok(this.mapper.Map<SupplierDb[], SupplierDto[]>(suppliers));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<SupplierDto> GetById(int id)
        {
            try
            {
                var supplier = this.supplierService.GetById(id);
                if (supplier != null)
                {
                    return Ok(this.mapper.Map<SupplierDb, SupplierDto>(supplier));
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
        public ActionResult<ProductDto[]> GetProductsBySupplierId(int id)
        {
            try
            {
                var products = this.productService.GetAllBySupplierId(id);
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
        public ActionResult<SupplierDto> Create(SupplierDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<SupplierDto, SupplierDb>(model);
                    var supplier = this.supplierService.Create(mappedModel);
                    return Ok(this.mapper.Map<SupplierDb, SupplierDto>(supplier));
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
        public ActionResult<SupplierDto> Update(SupplierDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<SupplierDto, SupplierDb>(model);
                    var supplier = this.supplierService.Update(mappedModel);

                    if (supplier != null)
                    {
                        return Ok(this.mapper.Map<SupplierDb, SupplierDto>(supplier));
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
        public ActionResult<SupplierDto> Delete(int id)
        {
            try
            {
                var supplier = this.supplierService.Delete(id);
                if (supplier != null)
                {
                    return Ok(this.mapper.Map<SupplierDb, SupplierDto>(supplier));
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
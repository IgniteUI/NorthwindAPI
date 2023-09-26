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
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public SuppliersController(SupplierService SupplierService, IMapper mapper, ILogger logger)
        {
            this.supplierService = SupplierService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public ActionResult<ProductDto[]> GetProductsBySupplierId(int id)
        {
            try
            {
                var supplier = this.supplierService.GetById(id);
                if (supplier != null)
                {
                    return Ok(this.mapper.Map<ProductDb[], ProductDto[]>(supplier.Products.ToArray()));
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
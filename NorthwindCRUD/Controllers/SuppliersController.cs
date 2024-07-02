namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    [Route("[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService supplierService;
        private readonly ProductService productService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<SuppliersController> logger;

        public SuppliersController(SupplierService supplierService, ProductService productService, PagingService pagingService, IMapper mapper, ILogger<SuppliersController> logger)
        {
            this.supplierService = supplierService;
            this.productService = productService;
            this.pagingService = pagingService;
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

        /// <summary>
        /// Fetches all suppliers or a page of suppliers based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the suppliers. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of suppliers to fetch. If this parameter is not provided, all suppliers are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the suppliers by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedSuppliers")]
        public ActionResult<PagedResultDto<SupplierDto>> GetAllSuppliers(
            [FromQuery][SwaggerParameter("The number of records to skip before starting to fetch the suppliers. If this parameter is not provided, fetching starts from the beginning.")] int? skip,
            [FromQuery][SwaggerParameter("The maximum number of suppliers to fetch. If this parameter is not provided, all suppliers are fetched.")] int? top,
            [FromQuery][SwaggerParameter("A comma-separated list of fields to order the suppliers by, along with the sort direction (e.g., 'field1 asc, field2 desc').")] string? orderBy)
        {
            try
            {
                // Retrieve all suppliers
                var suppliers = this.supplierService.GetAllAsQueryable();

                // Get paged data
                var pagedResult = pagingService.FetchPagedDataWithSkip<SupplierDb, SupplierDto>(suppliers, skip, top, orderBy);

                return Ok(pagedResult);
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
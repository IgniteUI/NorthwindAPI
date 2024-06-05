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
    public class EmployeesController : Controller
    {
        private readonly EmployeeService employeeService;
        private readonly EmployeeTerritoryService employeeTerritoryService;
        private readonly OrderService ordersService;
        private readonly PagingService pagingService;
        private readonly IMapper mapper;
        private readonly ILogger<EmployeesController> logger;

        public EmployeesController(EmployeeService employeeService, EmployeeTerritoryService employeeTerritoryService, OrderService ordersService, PagingService pagingService, IMapper mapper, ILogger<EmployeesController> logger)
        {
            this.employeeService = employeeService;
            this.employeeTerritoryService = employeeTerritoryService;
            this.pagingService = pagingService;
            this.ordersService = ordersService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<EmployeeDto[]> GetAll()
        {
            try
            {
                var employees = this.employeeService.GetAll();
                return Ok(this.mapper.Map<EmployeeDb[], EmployeeDto[]>(employees));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetches all employees or a page of employees based on the provided parameters.
        /// </summary>
        /// <param name="skip">The number of records to skip before starting to fetch the employees. If this parameter is not provided, fetching starts from the beginning.</param>
        /// <param name="top">The maximum number of employees to fetch. If this parameter is not provided, all employees are fetched.</param>
        /// <param name="orderBy">A comma-separated list of fields to order the employees by, along with the sort direction (e.g., "field1 asc, field2 desc").</param>
        /// <returns>A PagedResultDto object containing the fetched T and the total record count.</returns>
        [HttpGet("GetPagedEmployees")]
        public ActionResult<PagedResultDto<EmployeeDto>> GetAllEmployees(int? skip, int? top, string? orderBy)
        {
            try
            {
                // Retrieve all employees
                var employees = this.employeeService.GetAll();

                // Get paged data
                var pagedResult = pagingService.GetPagedData<EmployeeDb, EmployeeDto>(employees, skip, top, orderBy);

                return Ok(pagedResult);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<EmployeeDto> GetById(int id)
        {
            try
            {
                var employee = this.employeeService.GetById(id);

                if (employee != null)
                {
                    return Ok(this.mapper.Map<EmployeeDb, EmployeeDto>(employee));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Superior")]
        public ActionResult<EmployeeDto> GetSuperiorById(int id)
        {
            try
            {
                var employee = this.employeeService.GetById(id);

                if (employee != null)
                {
                    var superior = this.employeeService.GetById(employee.ReportsTo);

                    if (superior != null)
                    {
                        return Ok(this.mapper.Map<EmployeeDb, EmployeeDto>(superior));
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

        [HttpGet("{id}/Subordinates")]
        public ActionResult<EmployeeDto[]> GetSubordinatesById(int id)
        {
            try
            {
                return Ok(this.mapper.Map<EmployeeDb[], EmployeeDto[]>(this.employeeService.GetEmployeesByReportsTo(id)));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Orders")]
        public ActionResult<OrderDto[]> GetOrdersByEmployeeId(int id)
        {
            try
            {
                var orders = this.ordersService.GetOrdersByEmployeeId(id);
                return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(orders));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Teritories")]
        public ActionResult<EmployeeDto[]> GetTeritoriesByEmployeeId(int id)
        {
            try
            {
                var teritories = this.employeeTerritoryService.GetTeritoriesByEmployeeId(id);
                if (teritories == null)
                {
                    return NotFound($"No territories for employee {id}");
                }

                return Ok(this.mapper.Map<TerritoryDb[], TerritoryDto[]>(teritories));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("Teritory")]
        [Authorize]
        public ActionResult<EmployeeTerritoryDto> AddTerritoryToEmployee(EmployeeTerritoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<EmployeeTerritoryDto, EmployeeTerritoryDb>(model);
                    var employeeTerrirtory = this.employeeTerritoryService.AddTerritoryToEmployee(mappedModel);
                    return Ok(this.mapper.Map<EmployeeTerritoryDb, EmployeeTerritoryDto>(employeeTerrirtory));
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                logger.LogError(exception.Message);
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<EmployeeDto> Create(EmployeeDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<EmployeeDto, EmployeeDb>(model);
                    var employee = this.employeeService.Create(mappedModel);
                    return Ok(this.mapper.Map<EmployeeDb, EmployeeDto>(employee));
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
        public ActionResult<EmployeeDto> Update(EmployeeDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<EmployeeDto, EmployeeDb>(model);
                    var employee = this.employeeService.Update(mappedModel);

                    if (employee != null)
                    {
                        return Ok(this.mapper.Map<EmployeeDb, EmployeeDto>(employee));
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
        public ActionResult<EmployeeDto> Delete(int id)
        {
            try
            {
                var employee = this.employeeService.Delete(id);

                if (employee != null)
                {
                    return Ok(this.mapper.Map<EmployeeDb, EmployeeDto>(employee));
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

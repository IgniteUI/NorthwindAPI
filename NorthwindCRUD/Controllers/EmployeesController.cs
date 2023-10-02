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
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public EmployeesController(EmployeeService employeeService, EmployeeTerritoryService employeeTerritoryService, OrderService ordersService, IMapper mapper, ILogger logger)
        {
            this.employeeService = employeeService;
            this.employeeTerritoryService = employeeTerritoryService;
            this.ordersService = ordersService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
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

        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public ActionResult<OrderDto[]> GetOrdersByEmployeeId(int id)
        {
            try
            {
                var orders = this.ordersService.GetOrdersByEmployeeId(id);
                if (orders != null)
                {
                    return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(orders));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Teritories")]
        [Authorize]
        public ActionResult<EmployeeDto[]> GetTeritoriesByEmployeeId(int id)
        {
            try
            {
                var teritories = this.employeeTerritoryService.GetTeritoriesByEmployeeId(id);
                if (teritories != null)
                {
                    return Ok(this.mapper.Map<TerritoryDb[], TerritoryDto[]>(teritories));
                }

                return NotFound();
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

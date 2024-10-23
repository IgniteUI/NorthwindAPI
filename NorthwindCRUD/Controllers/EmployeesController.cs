namespace NorthwindCRUD.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : BaseNorthwindAPIController<EmployeeDto, EmployeeDb, int>
    {
        private readonly EmployeeService employeeService;
        private readonly OrderService ordersService;
        private readonly EmployeeTerritoryService employeeTerritoryService;
        private readonly ILogger<EmployeesController> logger;

        public EmployeesController(EmployeeService employeeService, EmployeeTerritoryService employeeTerritoryService, OrderService ordersService, ILogger<EmployeesController> logger)
            : base(employeeService)
        {
            this.employeeService = employeeService;
            this.employeeTerritoryService = employeeTerritoryService;
            this.ordersService = ordersService;
            this.logger = logger;
        }

        [HttpGet("{id}/Superior")]
        public ActionResult<EmployeeDto> GetSuperiorById(int id)
        {
            var employee = this.baseDbService.GetById(id);

            if (employee != null)
            {
                var superior = this.baseDbService.GetById(employee.ReportsTo);

                if (superior != null)
                {
                    return Ok(superior);
                }
            }

            return NotFound();
        }

        [HttpGet("{id}/Subordinates")]
        public ActionResult<EmployeeDto[]> GetSubordinatesById(int id)
        {
            return Ok(this.employeeService.GetEmployeesByReportsTo(id));
        }

        [HttpGet("{id}/Orders")]
        public ActionResult<OrderDto[]> GetOrdersByEmployeeId(int id)
        {
            var orders = this.ordersService.GetOrdersByEmployeeId(id);
            return Ok(orders);
        }

        [HttpGet("{id}/Territories")]
        public ActionResult<EmployeeDto[]> GetTerritoriesByEmployeeId(int id)
        {
            var territories = this.employeeTerritoryService.GetTeritoriesByEmployeeId(id);
            if (territories == null)
            {
                return NotFound($"No territories for employee {id}");
            }

            return Ok(territories);
        }

        [HttpPost("Teritory")]
        [Authorize]
        public ActionResult<EmployeeTerritoryDto> AddTerritoryToEmployee(EmployeeTerritoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employeeTerritory = this.employeeTerritoryService.AddTerritoryToEmployee(model);
                    return Ok(employeeTerritory);
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                this.logger.LogError(exception.Message);
                return StatusCode(400, exception.Message);
            }
        }
    }
}

namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService employeeService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public EmployeeController(EmployeeService employeeService, IMapper mapper, ILogger logger)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<EmployeeInputModel[]> GetAll()
        {
            try
            {
                var employees = this.employeeService.GetAll();
                return Ok(this.mapper.Map<EmployeeDb[], EmployeeInputModel[]>(employees));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<EmployeeInputModel> GetById(int id)
        {
            try
            {
                var employee = this.employeeService.GetById(id);

                if (employee != null)
                {
                    return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
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
        public ActionResult<EmployeeInputModel> Create(EmployeeInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<EmployeeInputModel, EmployeeDb>(model);
                    var employee = this.employeeService.Create(mappedModel);
                    return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
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
        public ActionResult<EmployeeInputModel> Update(EmployeeInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<EmployeeInputModel, EmployeeDb>(model);
                    var employee = this.employeeService.Update(mappedModel);

                    if (employee != null)
                    {
                        return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
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
        public ActionResult<EmployeeInputModel> Delete(int id)
        {
            try
            {
                var employee = this.employeeService.Delete(id);

                if (employee != null)
                {
                    return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
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

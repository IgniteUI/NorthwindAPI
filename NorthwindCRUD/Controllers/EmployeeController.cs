namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
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

        public EmployeeController(EmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<EmployeeInputModel[]> GetAll()
        {
            var employees = this.employeeService.GetAll();
            return Ok(this.mapper.Map<EmployeeDb[], EmployeeInputModel[]>(employees));
        }

        [HttpGet("{id}")]
        public ActionResult<EmployeeInputModel> GetById(int id)
        {
            var employee = this.employeeService.GetById(id);

            if (employee != null)
            {
                return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
            }

            return NotFound();
        }
        
        [HttpPost]
        public ActionResult<EmployeeInputModel> Create(EmployeeInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<EmployeeInputModel, EmployeeDb>(model);
                var employee = this.employeeService.Create(mappedModel);
                return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public ActionResult<EmployeeInputModel> Update(EmployeeInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<EmployeeInputModel, EmployeeDb>(model);
                var employee = this.employeeService.Update(mappedModel);
                return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
            }

            return BadRequest(ModelState);

            
        }

        [HttpDelete("{id}")]
        public ActionResult<EmployeeInputModel> Delete(int id)
        {
            var employee = this.employeeService.Delete(id);

            if (employee != null)
            {
                return Ok(this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee));
            }

            return NotFound();
        }
    }
}

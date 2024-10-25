namespace NorthwindCRUD.Controllers
{
    using System.Threading.Tasks;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [GraphRoute("employee")]
    public class EmployeeGraphController : GraphController
    {
        private readonly EmployeeService employeeService;

        public EmployeeGraphController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [Query]
        public EmployeeDto[] GetAll()
        {
            var employees = this.employeeService.GetAll();
            return employees;
        }

        [Query]
        public EmployeeDto? GetById(int id)
        {
            var employee = this.employeeService.GetById(id);

            return employee;
        }

        [Mutation]
        public async Task<EmployeeDto> Create(EmployeeDto model)
        {
            var employee = await this.employeeService.Create(model);
            return employee;
        }

        [Mutation]
        public async Task<EmployeeDto?> Update(EmployeeDto model)
        {
            var employee = await this.employeeService.Update(model);
            return employee;
        }

        [Mutation]
        public EmployeeDto? Delete(int id)
        {
            var employee = this.employeeService.Delete(id);

            return employee;
        }
    }
}

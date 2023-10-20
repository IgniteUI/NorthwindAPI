namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [GraphRoute("employee")]
    public class EmployeeGraphController : GraphController
    {
        private readonly EmployeeService employeeService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public EmployeeGraphController(EmployeeService employeeService, IMapper mapper, ILogger logger)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Query]
        public EmployeeDto[] GetAll()
        {
            var employees = this.employeeService.GetAll();
            return this.mapper.Map<EmployeeDb[], EmployeeDto[]>(employees);
        }

        [Query]
        public EmployeeDto? GetById(int id)
        {
            var employee = this.employeeService.GetById(id);

            if (employee != null)
            {
                return this.mapper.Map<EmployeeDb, EmployeeDto>(employee);
            }

            return null;
        }

        [Mutation]
        public EmployeeDto Create(EmployeeDto model)
        {
            var mappedModel = this.mapper.Map<EmployeeDto, EmployeeDb>(model);
            var employee = this.employeeService.Create(mappedModel);
            return this.mapper.Map<EmployeeDb, EmployeeDto>(employee);
        }

        [Mutation]
        public EmployeeDto? Update(EmployeeDto model)
        {
            var mappedModel = this.mapper.Map<EmployeeDto, EmployeeDb>(model);
            var employee = this.employeeService.Update(mappedModel);
            return employee != null ? this.mapper.Map<EmployeeDb, EmployeeDto>(employee) : null;
        }

        [Mutation]
        public EmployeeDto? Delete(int id)
        {
            var employee = this.employeeService.Delete(id);

            if (employee != null)
            {
                return this.mapper.Map<EmployeeDb, EmployeeDto>(employee);
            }

            return null;
        }
    }
}

namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.DbModels;
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
        public EmployeeInputModel[] GetAll()
        {
            var employees = this.employeeService.GetAll();
            return this.mapper.Map<EmployeeDb[], EmployeeInputModel[]>(employees);
        }

        [Query]
        public EmployeeInputModel GetById(int id)
        {
            var employee = this.employeeService.GetById(id);

            if (employee != null)
            {
                return this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee);
            }

            return null;
        }
        
        [Mutation]
        public EmployeeInputModel Create(EmployeeInputModel model)
        {
            var mappedModel = this.mapper.Map<EmployeeInputModel, EmployeeDb>(model);
            var employee = this.employeeService.Create(mappedModel);
            return this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee);
        }

        [Mutation]
        public EmployeeInputModel Update(EmployeeInputModel model)
        {
            var mappedModel = this.mapper.Map<EmployeeInputModel, EmployeeDb>(model);
            var employee = this.employeeService.Update(mappedModel);
            return this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee);
        }

        [Mutation]
        public EmployeeInputModel Delete(int id)
        {
            var employee = this.employeeService.Delete(id);

            if (employee != null)
            {
                return this.mapper.Map<EmployeeDb, EmployeeInputModel>(employee);
            }

            return null;
        }
    }
}

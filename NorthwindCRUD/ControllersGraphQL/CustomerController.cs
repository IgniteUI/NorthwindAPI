namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [GraphRoute("customer")]
    public class CustomerGraphController : GraphController
    {
        private readonly CustomerService customerService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CustomerGraphController(CustomerService customerService, IMapper mapper, ILogger logger)
        {
            this.customerService = customerService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Query]
        public CustomerInputModel[] GetAll()
        {
            var customers = this.customerService.GetAll();
            return this.mapper.Map<CustomerDb[], CustomerInputModel[]>(customers);
        }

        [Query]
        public CustomerInputModel GetById(string id)
        {
            var customer = this.customerService.GetById(id);

            if (customer != null)
            {
                return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
            }

            return null;
        }

        [Mutation]
        public CustomerInputModel Create(CustomerInputModel model)
        {

            var mappedModel = this.mapper.Map<CustomerInputModel, CustomerDb>(model);
            var customer = this.customerService.Create(mappedModel);
            return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
        }

        [Mutation]
        public CustomerInputModel Update(CustomerInputModel model)
        {
            var mappedModel = this.mapper.Map<CustomerInputModel, CustomerDb>(model);
            var customer = this.customerService.Update(mappedModel);
            return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
        }

        [Mutation]
        public CustomerInputModel Delete(string id)
        {
            var customer = this.customerService.Delete(id);

            if (customer != null)
            {
                return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
            }

            return null;
        }
    }
}

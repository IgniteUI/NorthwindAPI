using AutoMapper;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Models.InputModels;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Controllers
{
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
        public CustomerDto[] GetAll()
        {
            var customers = this.customerService.GetAll();
            return this.mapper.Map<CustomerDb[], CustomerDto[]>(customers);
        }

        [Query]
        public CustomerDto? GetById(string id)
        {
            var customer = this.customerService.GetById(id);

            if (customer != null)
            {
                return this.mapper.Map<CustomerDb, CustomerDto>(customer);
            }

            return null;
        }

        [Mutation]
        public CustomerDto Create(CustomerDto model)
        {
            var mappedModel = this.mapper.Map<CustomerDto, CustomerDb>(model);
            var customer = this.customerService.Create(mappedModel);
            return this.mapper.Map<CustomerDb, CustomerDto>(customer);
        }

        [Mutation]
        public CustomerDto? Update(string id, CustomerDto model)
        {
            var mappedModel = this.mapper.Map<CustomerDto, CustomerDb>(model);
            var customer = this.customerService.Update(id, mappedModel);
            return customer != null ? this.mapper.Map<CustomerDb, CustomerDto>(customer) : null;
        }

        [Mutation]
        public CustomerDto? Delete(string id)
        {
            var customer = this.customerService.Delete(id);

            if (customer != null)
            {
                return this.mapper.Map<CustomerDb, CustomerDto>(customer);
            }

            return null;
        }
    }
}

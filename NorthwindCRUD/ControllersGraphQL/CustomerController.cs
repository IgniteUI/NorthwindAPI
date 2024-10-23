using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Controllers
{
    [GraphRoute("customer")]
    public class CustomerGraphController : GraphController
    {
        private readonly CustomerService customerService;

        public CustomerGraphController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        [Query]
        public CustomerDto[] GetAll()
        {
            var customers = this.customerService.GetAll();
            return customers;
        }

        [Query]
        public CustomerDto? GetById(string id)
        {
            var customer = this.customerService.GetById(id);
            return customer;
        }

        [Mutation]
        public async Task<CustomerDto> Create(CustomerDto model)
        {
            return await this.customerService.Create(model);
        }

        [Mutation]
        public async Task<CustomerDto?> Update(CustomerDto model)
        {
            return await this.customerService.Update(model);
        }

        [Mutation]
        public CustomerDto? Delete(string id)
        {
            var customer = this.customerService.Delete(id);
            return customer;
        }
    }
}

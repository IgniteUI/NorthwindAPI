namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {

        private readonly CustomerService customerService;
        private readonly IMapper mapper;

        public CustomerController(CustomerService customerService, IMapper mapper)
        {
            this.customerService = customerService;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<CustomerInputModel[]> GetAll()
        {
            var customers = this.customerService.GetAll();
            return this.mapper.Map<CustomerDb[], CustomerInputModel[]>(customers);
        }

        [HttpGet("{id}")]
        public ActionResult<CustomerInputModel> GetById(string id)
        {
            var customer = this.customerService.GetById(id);
            return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
        }

        [HttpPost]
        public ActionResult<CustomerInputModel> Create(CustomerInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<CustomerInputModel, CustomerDb>(model);
                var customer = this.customerService.Create(mappedModel);
                return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
            }

            return null;
        }

        [HttpPut]
        public ActionResult<CustomerInputModel> Update(CustomerInputModel model)
        {
            var mappedModel = this.mapper.Map<CustomerInputModel, CustomerDb>(model);
            var customer = this.customerService.Update(mappedModel);
            return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
        }

        [HttpDelete("{id}")]
        public ActionResult<CustomerInputModel> Delete(string id)
        {
            var customer = this.customerService.Delete(id);
            return this.mapper.Map<CustomerDb, CustomerInputModel>(customer);
        }
    }
}

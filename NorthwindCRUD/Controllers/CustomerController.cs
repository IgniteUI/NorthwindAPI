namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.Contracts;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomerService customerService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CustomerController(CustomerService customerService, IMapper mapper, ILogger logger)
        {
            this.customerService = customerService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<CustomerInputModel[]> GetAll()
        {
            try
            {
                var customers = this.customerService.GetAll();
                return Ok(this.mapper.Map<CustomerDb[], CustomerInputModel[]>(customers));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CustomerInputModel> GetById(string id)
        {
            try
            {
                var customer = this.customerService.GetById(id);

                if (customer != null)
                {
                    return Ok(this.mapper.Map<CustomerDb, CustomerInputModel>(customer));
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
        public ActionResult<CustomerInputModel> Create(CustomerInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CustomerInputModel, CustomerDb>(model);
                    var customer = this.customerService.Create(mappedModel);
                    return Ok(this.mapper.Map<CustomerDb, CustomerInputModel>(customer));
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
        public ActionResult<CustomerInputModel> Update(CustomerInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CustomerInputModel, CustomerDb>(model);
                    var customer = this.customerService.Update(mappedModel);

                    if (customer != null)
                    {
                        return Ok(this.mapper.Map<CustomerDb, CustomerInputModel>(customer));
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
        public ActionResult<CustomerInputModel> Delete(string id)
        {
            try
            {
                var customer = this.customerService.Delete(id);
                if (customer != null)
                {
                    return Ok(this.mapper.Map<CustomerDb, CustomerInputModel>(customer));
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

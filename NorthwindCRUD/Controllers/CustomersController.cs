namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly CustomerService customerService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CustomersController(CustomerService customerService, IMapper mapper, ILogger logger)
        {
            this.customerService = customerService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<CustomerDto[]> GetAll()
        {
            try
            {
                var customers = this.customerService.GetAll();
                return Ok(this.mapper.Map<CustomerDb[], CustomerDto[]>(customers));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CustomerDto> GetById(string id)
        {
            try
            {
                var customer = this.customerService.GetById(id);

                if (customer != null)
                {
                    return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Orders")]
        [Authorize]
        public ActionResult<OrderDto[]> GetOrdersByCustomerId(string id)
        {
            try
            {
                var customer = this.customerService.GetById(id);
                if (customer != null)
                {
                    return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(customer.Orders.ToArray()));
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
        public ActionResult<CustomerDto> Create(CustomerDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CustomerDto, CustomerDb>(model);
                    var customer = this.customerService.Create(mappedModel);
                    return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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
        public ActionResult<CustomerDto> Update(CustomerDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CustomerDto, CustomerDb>(model);
                    var customer = this.customerService.Update(mappedModel);

                    if (customer != null)
                    {
                        return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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
        public ActionResult<CustomerDto> Delete(string id)
        {
            try
            {
                var customer = this.customerService.Delete(id);
                if (customer != null)
                {
                    return Ok(this.mapper.Map<CustomerDb, CustomerDto>(customer));
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

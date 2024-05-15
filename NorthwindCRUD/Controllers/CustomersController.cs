﻿namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.Errors;
    using NorthwindCRUD.Services;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly CustomerService customerService;
        private readonly OrderService orderService;
        private readonly IMapper mapper;
        private readonly ILogger<CustomersController> logger;

        public CustomersController(CustomerService customerService, OrderService orderService, IMapper mapper, ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.orderService = orderService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
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
        public ActionResult<OrderDto[]> GetOrdersByCustomerId(string id)
        {
            try
            {
                var orders = this.orderService.GetOrdersByCustomerId(id);
                return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(orders));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [SwaggerResponse(400, "Invalid input data!", typeof(Errors), "text/json")]
        [SwaggerResponse(401, "Unauthorized!", typeof(CustomError), "text/json")]

        // [Authorize]
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
        [SwaggerResponse(401, "Not authenticated!", typeof(CustomError), "text/json")]
        [SwaggerResponse(404, "Your client is not found!", typeof(CustomError), "text/json")]

        // [Authorize]
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

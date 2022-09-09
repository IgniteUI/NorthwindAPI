namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrderController(OrderService orderService, IMapper mapper, ILogger logger)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<OrderInputModel[]> GetAll()
        {
            try
            {
                var orders = this.orderService.GetAll();
                return Ok(this.mapper.Map<OrderDb[], OrderInputModel[]>(orders));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
        
        [HttpGet("{id}")]
        public ActionResult<OrderInputModel> GetById(int id)
        {
            try
            {
                var order = this.orderService.GetById(id);

                if (order != null)
                {
                    return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
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
        public ActionResult<OrderInputModel> Create(OrderInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<OrderInputModel, OrderDb>(model);
                    var order = this.orderService.Create(mappedModel);
                    return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
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
        public ActionResult<OrderInputModel> Update(OrderInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<OrderInputModel, OrderDb>(model);
                    var order = this.orderService.Update(mappedModel);
                    return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
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
        public ActionResult<OrderInputModel> Delete(int id)
        {
            try
            {
                var order = this.orderService.Delete(id);

                if (order != null)
                {
                    return Ok(this.mapper.Map<OrderDb, OrderInputModel>(order));
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

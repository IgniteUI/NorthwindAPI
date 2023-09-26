namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class ShippersController : ControllerBase
    {
        private readonly ShipperService ShipperService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ShippersController(ShipperService ShipperService, IMapper mapper, ILogger logger)
        {
            this.ShipperService = ShipperService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<ShipperDto[]> GetAll()
        {
            try
            {
                var Shippers = this.ShipperService.GetAll();
                return Ok(this.mapper.Map<ShipperDb[], ShipperDto[]>(Shippers));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }

        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ShipperDto> GetById(int id)
        {
            try
            {
                var category = this.ShipperService.GetById(id);
                if (category != null)
                {
                    return Ok(this.mapper.Map<ShipperDb, ShipperDto>(category));
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
        public ActionResult<OrderDto[]> GetOrdersByShipperId(int id)
        {
            try
            {
                var shipper = this.ShipperService.GetById(id);
                if (shipper != null)
                {
                    return Ok(this.mapper.Map<OrderDb[], OrderDto[]>(shipper.Orders.ToArray()));
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
        public ActionResult<ShipperDto> Create(ShipperDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<ShipperDto, ShipperDb>(model);
                    var shipper = this.ShipperService.Create(mappedModel);
                    return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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
        public ActionResult<ShipperDto> Update(ShipperDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<ShipperDto, ShipperDb>(model);
                    var shipper = this.ShipperService.Update(mappedModel);

                    if (shipper != null)
                    {
                        return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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
        public ActionResult<ShipperDto> Delete(int id)
        {
            try
            {
                var shipper = this.ShipperService.Delete(id);
                if (shipper != null)
                {
                    return Ok(this.mapper.Map<ShipperDb, ShipperDto>(shipper));
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
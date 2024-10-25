using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NorthwindCRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly SalesService salesService;
        private readonly IMapper mapper;
        private readonly ILogger<SalesController> logger;

        public SalesController(SalesService salesService, IMapper mapper, ILogger<SalesController> logger)
        {
            this.salesService = salesService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("ByCategory")]
        [Authorize]
        public ActionResult<SalesDto[]> GetSalesByCategoryAndYear([FromQuery][Required] string categoryName, [FromQuery] int? orderYear = null)
        {
            try
            {
                var response = this.salesService.GetSalesDataByCategoryAndYear(categoryName, orderYear);
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("ByCountry/{country}")]
        [Authorize]
        public ActionResult<SalesDto[]> GetSalesByCountry(
            string country,
            [FromQuery][Required][DataType(DataType.Date)][SwaggerParameter("Start date in YYYY-MM-DD format")] string startDate,
            [FromQuery][Required][DataType(DataType.Date)][SwaggerParameter("End date in YYYY-MM-DD format")] string endDate)
        {
            try
            {
                var salesData = this.salesService.RetrieveSalesDataByCountry(startDate, endDate, country);

                if (salesData == null)
                {
                    return NotFound("No sales data found for the specified parameters.");
                }

                return Ok(salesData);
            }
            catch (ArgumentException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("ByYear/{year}")]
        [Authorize]
        public ActionResult<SalesDto[]> GetSalesByYear(
            int year,
            [FromQuery] int startMounth,
            [FromQuery] int endMounth)
        {
            try
            {
                var salesData = this.salesService.RetrieveSalesDataByYear(year, startMounth, endMounth);

                if (salesData == null)
                {
                    return NotFound("No sales data found for the specified parameters.");
                }

                return Ok(salesData);
            }
            catch (ArgumentException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
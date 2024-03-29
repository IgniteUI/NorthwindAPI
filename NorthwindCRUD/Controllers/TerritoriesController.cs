﻿namespace NorthwindCRUD.Controllers
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
    public class TerritoriesController : ControllerBase
    {
        private readonly TerritoryService territoryService;
        private readonly RegionService regionService;
        private readonly EmployeeTerritoryService employeeTerritoryService;
        private readonly IMapper mapper;
        private readonly ILogger<TerritoriesController> logger;

        public TerritoriesController(TerritoryService territoryService, EmployeeTerritoryService employeeTerritoryService, RegionService regionService, IMapper mapper, ILogger<TerritoriesController> logger)
        {
            this.territoryService = territoryService;
            this.regionService = regionService;
            this.employeeTerritoryService = employeeTerritoryService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<TerritoryDto[]> GetAll()
        {
            try
            {
                var territories = this.territoryService.GetAll();
                return Ok(this.mapper.Map<TerritoryDb[], TerritoryDto[]>(territories));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TerritoryDto> GetById(string id)
        {
            try
            {
                var teritory = this.territoryService.GetById(id);
                if (teritory != null)
                {
                    return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(teritory));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Employees")]
        public ActionResult<EmployeeDto[]> GetEmployeesByTerritory(string id)
        {
            try
            {
                var employees = this.employeeTerritoryService.GetEmployeesByTerritoryId(id);
                if (employees == null)
                {
                    return NotFound($"No employees for territory {id}");
                }

                return Ok(this.mapper.Map<EmployeeDb[], EmployeeDto[]>(employees));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}/Region")]
        public ActionResult<RegionDto[]> GetRegionByTerritory(string id)
        {
            try
            {
                var territory = this.territoryService.GetById(id);
                if (territory != null)
                {
                    var region = this.regionService.GetById(territory.RegionId ?? default);

                    if (region != null)
                    {
                        return Ok(this.mapper.Map<RegionDb, RegionDto>(region));
                    }
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
        public ActionResult<TerritoryDto> Create(TerritoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<TerritoryDto, TerritoryDb>(model);
                    var teritory = this.territoryService.Create(mappedModel);
                    return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(teritory));
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<TerritoryDto> Update(TerritoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<TerritoryDto, TerritoryDb>(model);
                    var territory = this.territoryService.Update(mappedModel);

                    if (territory != null)
                    {
                        return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(territory));
                    }

                    return NotFound();
                }

                return BadRequest(ModelState);
            }
            catch (InvalidOperationException exception)
            {
                return StatusCode(400, exception.Message);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<TerritoryDto> Delete(string id)
        {
            try
            {
                var territory = this.territoryService.Delete(id);
                if (territory != null)
                {
                    return Ok(this.mapper.Map<TerritoryDb, TerritoryDto>(territory));
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